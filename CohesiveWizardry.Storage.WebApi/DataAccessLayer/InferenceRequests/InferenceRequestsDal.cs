using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using CohesiveWizardry.Common.Configuration;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions;
using CohesiveWizardry.Common.Serialization;
using CohesiveWizardry.Storage.Dtos.Requests.InferenceRequests;
using CohesiveWizardry.Storage.Dtos.Responses.InferenceRequests;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;

namespace CohesiveWizardry.Storage.WebApi.DataAccessLayer.InferenceRequests
{
    public class InferenceRequestsDal : IInferenceRequestsDal
    {
        private const int NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION = 5000;
        private const int NB_MS_UNREACHABLE_DB_ON_STARTUP = 30000;
        private string InferenceRequestTableName = "InferenceRequests";
        private AWSCredentials AWSCredentials = new BasicAWSCredentials("local", "local");
        private string DynamoDBUri = "http://127.0.0.1:8822";
        private IAmazonDynamoDB dynamoDBClient = null;

        public InferenceRequestsDal()
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

                    if (!tables.TableNames.Contains(InferenceRequestTableName))
                    {
                        await CreateTable(new CreateTableRequest(InferenceRequestTableName, new List<KeySchemaElement>
                        {
                            new KeySchemaElement("inferenceRequestId", KeyType.HASH),
                        }, new List<AttributeDefinition>
                        {
                            new AttributeDefinition("inferenceRequestId", ScalarAttributeType.S),
                        }, new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 1000,
                            WriteCapacityUnits = 1000,
                        }){});
                    }
                } catch (OperationCanceledException e)
                {
                    string errMessage = $"The database was unreachable for [{NB_MS_UNREACHABLE_DB_ON_STARTUP}] ms on startup. aborting.";
                    LoggingManager.LogToFile("1dc6a556-2939-4188-9d4e-8ee71f180ae7", errMessage);
                    throw new CommonException("2f019136-20a1-49b6-9634-803d0fb3629e", errMessage, e);
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
        public async Task<GetInferenceRequestResponseDto> GetInferenceRequestAsync(string InferenceRequestId)
        {
            if (string.IsNullOrWhiteSpace(InferenceRequestId))
                throw new ArgumentNullException(nameof(InferenceRequestId));

            try
            {
                var InferenceRequestItemsResult = await dynamoDBClient.GetItemAsync(new GetItemRequest
                {
                    TableName = InferenceRequestTableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "inferenceRequestId", new AttributeValue { S = InferenceRequestId } },
                    },
                }).ConfigureAwait(false);

                if (InferenceRequestItemsResult.Item == null || InferenceRequestItemsResult.Item.Count <= 0)
                    return null; // Item not found

                var responseDocument = Amazon.DynamoDBv2.DocumentModel.Document.FromAttributeMap(InferenceRequestItemsResult.Item);

                var jsonResponse = responseDocument.ToJson();
                var InferenceRequestDto = JsonCommonSerializer.DeserializeFromString<GetInferenceRequestResponseDto>(jsonResponse);

                return InferenceRequestDto;

            } catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION);

                throw;
            } catch (Exception ex)
            {
                // wrap exception
                throw new CommonException("abeabac1-6da1-43f9-bc81-3721f065d9f8", $"Unhandled exception when querying database. Failed to get InferenceRequest matching InferenceRequestId [{InferenceRequestId}] from storage. Exception message [{ex.Message}].", ex);
            }
        }

        /// <inheritdoc />
        public async Task<AddInferenceRequestResponseDto> AddInferenceRequestAsync(AddInferenceRequestDto addInferenceRequestDto)
        {
            if (addInferenceRequestDto == null)
                throw new ArgumentNullException(nameof(addInferenceRequestDto));

            var utcNow = DateTime.UtcNow;
            addInferenceRequestDto.CreatedAtUtc = utcNow;
            addInferenceRequestDto.LastModifiedAtUtc = utcNow;
            addInferenceRequestDto.Revision = 0;

            try
            {
                var InferenceRequestItemsResult = await dynamoDBClient.PutItemAsync(new PutItemRequest
                {
                    TableName = InferenceRequestTableName,
                    Item = addInferenceRequestDto.ToDocument(null).ToAttributeMap(),
                    ConditionExpression = "inferenceRequestId <> :InferenceRequestId",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":InferenceRequestId", new AttributeValue { S = addInferenceRequestDto.Id } },
                    },
                }).ConfigureAwait(false);


                if (InferenceRequestItemsResult == null)
                    return null; // Item not found

                var serializedData = JsonCommonSerializer.SerializeToString(addInferenceRequestDto);
                var InferenceRequestDto = JsonCommonSerializer.DeserializeFromString<AddInferenceRequestResponseDto>(serializedData);

                return InferenceRequestDto;
            } catch (ConditionalCheckFailedException)
            {
                throw new CommonException("ad796cfc-a847-464f-a495-083a70c101b8", $"InferenceRequest was created by another instance. InferenceRequest [{addInferenceRequestDto.Id}] won't be created to avoid duplicates.");
            } catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION);

                throw;
            } catch (Exception ex)
            {
                // wrap exception
                throw new CommonException("c6243ffa-208d-45c6-850b-f7b7741c4176", $"Unhandled exception when querying database. Failed to add InferenceRequest of Name [{addInferenceRequestDto.Id}] to storage. Exception message [{ex.Message}].", ex);
            }
        }

        /// <inheritdoc />
        public async Task<UpdateInferenceRequestResponseDto> UpdateInferenceRequestAsync(UpdateInferenceRequestDto updateInferenceRequestDto)
        {
            if (updateInferenceRequestDto == null)
                throw new ArgumentNullException(nameof(updateInferenceRequestDto));

            long requestLastRevision = updateInferenceRequestDto.Revision;

            var utcNow = DateTime.UtcNow;
            updateInferenceRequestDto.LastModifiedAtUtc = utcNow;
            updateInferenceRequestDto.Revision++;

            try
            {
                var InferenceRequestItemsResult = await dynamoDBClient.PutItemAsync(new PutItemRequest
                {
                    TableName = InferenceRequestTableName,
                    Item = updateInferenceRequestDto.ToDocument(null).ToAttributeMap(),
                    ConditionExpression = "#InferenceRequestId = :InferenceRequestId AND #revision = :revision",
                    ExpressionAttributeNames = new Dictionary<string, string>
                    {
                        { "#revision", "revision" },
                        { "#InferenceRequestId", "inferenceRequestId" },

                    },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":revision", new AttributeValue { N = requestLastRevision.ToString() } },
                        { ":InferenceRequestId", new AttributeValue { S = updateInferenceRequestDto.Id } },
                    },
                }).ConfigureAwait(false);

                if (InferenceRequestItemsResult == null)
                    return null; // Item not found

                var serializedData = JsonCommonSerializer.SerializeToString(updateInferenceRequestDto);
                var InferenceRequestDto = JsonCommonSerializer.DeserializeFromString<UpdateInferenceRequestResponseDto>(serializedData);

                return InferenceRequestDto;
            } catch (ConditionalCheckFailedException)
            {
                throw new CommonException("ee6361c1-ff05-4663-b0ba-0f60832cfad8", $"Revision didn't match. InferenceRequest [{updateInferenceRequestDto.Id}] won't be updated.");
            } catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION);

                throw;
            } catch (Exception ex)
            {
                // wrap exception
                throw new CommonException("792e4f50-d459-4b40-bc9b-959d5e40ecd6", $"Unhandled exception when querying database. Failed to update InferenceRequest [{updateInferenceRequestDto.Id}] in storage. Exception message [{ex.Message}].", ex);
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteInferenceRequestAsync(string inferenceRequestId)
        {
            if (string.IsNullOrWhiteSpace(inferenceRequestId))
                throw new ArgumentNullException(nameof(inferenceRequestId));

            try
            {
                var response = await dynamoDBClient.DeleteItemAsync(new DeleteItemRequest
                {
                    TableName = InferenceRequestTableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "inferenceRequestId", new AttributeValue { S = inferenceRequestId } },
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
                throw new CommonException("d53e146c-50c0-403f-9a3d-ce2f835d1ae5", $"Unhandled exception when querying database. Failed to delete InferenceRequest matching InferenceRequestId [{inferenceRequestId}] from storage. Exception message [{ex.Message}].", ex);
            }
        }
    }
}
