using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using CohesiveWizardry.Common.Configuration;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions;
using CohesiveWizardry.Common.Serialization;
using CohesiveWizardry.Storage.Dtos.Requests.Conversations;
using CohesiveWizardry.Storage.Dtos.Responses.Conversations;

namespace CohesiveWizardry.Storage.WebApi.DataAccessLayer.Conversations
{
    public class ConversationsDal : IConversationsDal
    {
        private const int NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION = 5000;
        private const int NB_MS_UNREACHABLE_DB_ON_STARTUP = 30000;
        private string conversationTableName = "Conversations";
        private AWSCredentials AWSCredentials = new BasicAWSCredentials("local", "local");
        private string DynamoDBUri = "http://127.0.0.1:8822";
        private IAmazonDynamoDB dynamoDBClient = null;

        public ConversationsDal()
        {
            dynamoDBClient = InitClient();
            InitDatabase().Wait();
        }

        private async Task InitDatabase()
        {
            var config = CommonConfigurationManager.ReloadConfig();

            // Create a cancellationToken with a predefined waiting time to avoid infinitely waiting on the DB here. If it's down, we want to EXIT instead of freezing to avoid scaling snowball
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                try
                {
                    cancellationTokenSource.CancelAfter(NB_MS_UNREACHABLE_DB_ON_STARTUP);
                    ListTablesResponse tables = await dynamoDBClient.ListTablesAsync(cancellationTokenSource.Token).ConfigureAwait(false);

                    if (!tables.TableNames.Contains(conversationTableName))
                    {
                        await CreateTable(new CreateTableRequest(conversationTableName, new List<KeySchemaElement>
                        {
                            new KeySchemaElement("conversationId", KeyType.HASH),
                        }, new List<AttributeDefinition>
                        {
                            new AttributeDefinition("conversationId", ScalarAttributeType.S),
                        }, new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 1000,
                            WriteCapacityUnits = 1000,
                        })
                        { });
                    }
                } catch (OperationCanceledException e)
                {
                    string errMessage = $"The database was unreachable for [{NB_MS_UNREACHABLE_DB_ON_STARTUP}] ms on startup. aborting.";
                    LoggingManager.LogToFile("6a050bb1-40c5-4b58-841b-48b7843c856d", errMessage);
                    throw new CommonException("087bdf94-4250-46b1-a957-a8f10195d468", errMessage, e);
                } finally
                {
                    cancellationTokenSource.Dispose();
                }
            }
        }

        private async Task CreateTable(CreateTableRequest createTableRequest) => await dynamoDBClient.CreateTableAsync(createTableRequest).ConfigureAwait(false);

        private IAmazonDynamoDB InitClient()
        {
            return new AmazonDynamoDBClient(AWSCredentials, new AmazonDynamoDBConfig()
            {
                Timeout = new TimeSpan(0, 0, 10),
                ServiceURL = DynamoDBUri,
            });
        }

        /// <inheritdoc />
        public async Task<GetConversationResponseDto> GetConversationAsync(string conversationId)
        {
            if (string.IsNullOrWhiteSpace(conversationId))
                throw new ArgumentNullException(nameof(conversationId));

            try
            {
                var conversationItemsResult = await dynamoDBClient.GetItemAsync(new GetItemRequest
                {
                    TableName = conversationTableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "conversationId", new AttributeValue { S = conversationId } },
                    },
                }).ConfigureAwait(false);

                if (conversationItemsResult.Item == null || conversationItemsResult.Item.Count <= 0)
                    return null; // Item not found

                var responseDocument = Amazon.DynamoDBv2.DocumentModel.Document.FromAttributeMap(conversationItemsResult.Item);

                var jsonResponse = responseDocument.ToJson();
                var conversationDto = JsonCommonSerializer.DeserializeFromString<GetConversationResponseDto>(jsonResponse);

                return conversationDto;

            } catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION);

                throw;
            } catch (Exception ex)
            {
                // wrap exception
                throw new CommonException("c59a3ba7-4122-4061-ac72-83c48646744b", $"Unhandled exception when querying database. Failed to get Conversation matching ConversationId [{conversationId}] from storage. Exception message [{ex.Message}].", ex);
            }
        }

        /// <inheritdoc />
        public async Task<AddConversationResponseDto> AddConversationAsync(AddConversationRequestDto addConversationDto)
        {
            if (addConversationDto == null)
                throw new ArgumentNullException(nameof(addConversationDto));

            var utcNow = DateTime.UtcNow;
            addConversationDto.CreatedAtUtc = utcNow;
            addConversationDto.LastModifiedAtUtc = utcNow;
            addConversationDto.Revision = 0;

            try
            {
                var conversationItemsResult = await dynamoDBClient.PutItemAsync(new PutItemRequest
                {
                    TableName = conversationTableName,
                    Item = addConversationDto.ToDocument(null).ToAttributeMap(),
                    ConditionExpression = "conversationId <> :conversationId",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":conversationId", new AttributeValue { S = addConversationDto.Id } },
                    },
                }).ConfigureAwait(false);


                if (conversationItemsResult == null)
                    return null; // Item not found

                var serializedData = JsonCommonSerializer.SerializeToString(addConversationDto);
                var conversationDto = JsonCommonSerializer.DeserializeFromString<AddConversationResponseDto>(serializedData);

                return conversationDto;
            } catch (ConditionalCheckFailedException)
            {
                throw new CommonException("e9d95b2f-7fd2-4ee9-94d3-63e7302d8c22", $"Conversation was created by another instance. Conversation [{addConversationDto.Id}] won't be created to avoid duplicates.");
            } catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION);

                throw;
            } catch (Exception ex)
            {
                // wrap exception
                throw new CommonException("c794d10d-2c43-47d2-99a7-27d0b29962bd", $"Unhandled exception when querying database. Failed to add Conversation of Name [{addConversationDto.Id}] to storage. Exception message [{ex.Message}].", ex);
            }
        }

        /// <inheritdoc />
        public async Task<UpdateConversationResponseDto> UpdateConversationAsync(UpdateConversationRequestDto updateConversationDto)
        {
            if (updateConversationDto == null)
                throw new ArgumentNullException(nameof(updateConversationDto));

            long requestLastRevision = updateConversationDto.Revision;

            var utcNow = DateTime.UtcNow;
            updateConversationDto.LastModifiedAtUtc = utcNow;
            updateConversationDto.Revision++;

            try
            {
                var conversationItemsResult = await dynamoDBClient.PutItemAsync(new PutItemRequest
                {
                    TableName = conversationTableName,
                    Item = updateConversationDto.ToDocument(null).ToAttributeMap(),
                    ConditionExpression = "#conversationId = :conversationId AND #revision = :revision",
                    ExpressionAttributeNames = new Dictionary<string, string>
                    {
                        { "#revision", "revision" },
                        { "#conversationId", "conversationId" },

                    },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":revision", new AttributeValue { N = requestLastRevision.ToString() } },
                        { ":conversationId", new AttributeValue { S = updateConversationDto.Id } },
                    },
                }).ConfigureAwait(false);

                if (conversationItemsResult == null)
                    return null; // Item not found

                var serializedData = JsonCommonSerializer.SerializeToString(updateConversationDto);
                var conversationDto = JsonCommonSerializer.DeserializeFromString<UpdateConversationResponseDto>(serializedData);

                return conversationDto;
            } catch (ConditionalCheckFailedException)
            {
                throw new CommonException("aa46a5c3-a115-4467-b726-23915ad73e45", $"Revision didn't match. Conversation [{updateConversationDto.Id}] won't be updated.");
            } catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION);

                throw;
            } catch (Exception ex)
            {
                // wrap exception
                throw new CommonException("0a40be8e-1c9b-4967-bdcb-711d142c226c", $"Unhandled exception when querying database. Failed to update Conversation [{updateConversationDto.Id}] in storage. Exception message [{ex.Message}].", ex);
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteConversationAsync(string conversationId)
        {
            if (string.IsNullOrWhiteSpace(conversationId))
                throw new ArgumentNullException(nameof(conversationId));

            try
            {
                var response = await dynamoDBClient.DeleteItemAsync(new DeleteItemRequest
                {
                    TableName = conversationTableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "conversationId", new AttributeValue { S = conversationId } },
                    },
                }).ConfigureAwait(false);

                return response.HttpStatusCode != HttpStatusCode.NoContent;
            } catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION);

                throw;
            } catch (Exception ex)
            {
                // wrap exception
                throw new CommonException("2c52188f-36ef-408d-95b4-693282aa0e81", $"Unhandled exception when querying database. Failed to delete Conversation matching ConversationId [{conversationId}] from storage. Exception message [{ex.Message}].", ex);
            }
        }
    }
}
