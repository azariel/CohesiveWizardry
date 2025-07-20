using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Cohesive_rp_storage_dtos.Requests.Users;
using Cohesive_rp_storage_dtos.Response.Users;
using CohesiveWizardry.Common.Configuration;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions;
using CohesiveWizardry.Common.Serialization;

namespace CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users
{
    public class UsersDal : IUsersDal
    {
        private const int NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION = 5000;
        private const int NB_MS_UNREACHABLE_DB_ON_STARTUP = 30000;
        private string userTableName = "Users";
        private AWSCredentials AWSCredentials = new BasicAWSCredentials("local", "local");
        private string DynamoDBUri = "http://127.0.0.1:8822";
        private IAmazonDynamoDB dynamoDBClient = null;

        public UsersDal()
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

                    if (!tables.TableNames.Contains(userTableName))
                    {
                        await CreateTable(new CreateTableRequest(userTableName, new List<KeySchemaElement>
                        {
                            new KeySchemaElement("userId", KeyType.HASH),
                            //new KeySchemaElement("lastModifiedAt", KeyType.RANGE),
                        }, new List<AttributeDefinition>
                        {
                            new AttributeDefinition("userId", ScalarAttributeType.S),
                            //new AttributeDefinition("lastModifiedAt", ScalarAttributeType.S),
                        }, new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 1000,
                            WriteCapacityUnits = 1000,
                        })
                        {
                            //GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                            //{
                            //    new()
                            //    {
                            //        IndexName = config.StorageConfiguration.UsersWithMessagesToProcessIndexName,
                            //        KeySchema = new List<KeySchemaElement>
                            //        {
                            //            new("status", KeyType.HASH),
                            //            new("nbUnprocessedMessages", KeyType.RANGE),
                            //        },
                            //        Projection = new Projection
                            //        {
                            //            ProjectionType = ProjectionType.ALL,
                            //        },
                            //        ProvisionedThroughput = new ProvisionedThroughput
                            //        {
                            //            ReadCapacityUnits = 1000,
                            //            WriteCapacityUnits = 1000,
                            //        },
                            //    },
                            //},
                        });
                    }
                } catch (OperationCanceledException e)
                {
                    string errMessage = $"The database was unreachable for [{NB_MS_UNREACHABLE_DB_ON_STARTUP}] ms on startup. aborting.";
                    LoggingManager.LogToFile("b24ec527-f506-4d32-b143-77abb9b496af", errMessage);
                    throw new CommonException("56b71c8d-4929-48a2-8e29-2783168f74fc", errMessage, e);
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
        public async Task<GetUserResponseDto> GetUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            try
            {
                var userItemsResult = await dynamoDBClient.GetItemAsync(new GetItemRequest
                {
                    TableName = userTableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "userId", new AttributeValue { S = userId } },
                    },
                }).ConfigureAwait(false);

                if (userItemsResult.Item == null || userItemsResult.Item.Count <= 0)
                    return null; // Item not found

                var responseDocument = Amazon.DynamoDBv2.DocumentModel.Document.FromAttributeMap(userItemsResult.Item);

                var jsonResponse = responseDocument.ToJson();
                var userDto = JsonCommonSerializer.DeserializeFromString<GetUserResponseDto>(jsonResponse);

                return userDto;

            } catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION);

                throw;
            } catch (Exception ex)
            {
                // wrap exception
                throw new CommonException("f4eb8c84-e0af-43e1-842d-f52731e399a4", $"Unhandled exception when querying database. Failed to get User matching UserId [{userId}] from storage. Exception message [{ex.Message}].", ex);
            }
        }

        /// <inheritdoc />
        public async Task<AddUserResponseDto> AddUserAsync(AddUserRequestDto addUserDto)
        {
            if (addUserDto == null)
                throw new ArgumentNullException(nameof(addUserDto));

            var utcNow = DateTime.UtcNow;
            addUserDto.CreatedAtUtc = utcNow;
            addUserDto.LastModifiedAtUtc = utcNow;
            addUserDto.Revision = 0;

            try
            {
                var userItemsResult = await dynamoDBClient.PutItemAsync(new PutItemRequest
                {
                    TableName = userTableName,
                    Item = addUserDto.ToDocument(null).ToAttributeMap(),
                    ConditionExpression = "userId <> :userId",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":userId", new AttributeValue { S = addUserDto.Id } },
                    },
                }).ConfigureAwait(false);


                if (userItemsResult == null)
                    return null; // Item not found

                var serializedData = JsonCommonSerializer.SerializeToString(addUserDto);
                var userDto = JsonCommonSerializer.DeserializeFromString<AddUserResponseDto>(serializedData);

                return userDto;
            } catch (ConditionalCheckFailedException)
            {
                throw new CommonException("5bc620a5-c674-48eb-bdeb-9bef91d94a9e", $"User was created by another instance. User [{addUserDto.Username}] won't be created to avoid duplicates.");
            } catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION);

                throw;
            } catch (Exception ex)
            {
                // wrap exception
                throw new CommonException("ab29ed01-2081-4775-b6e8-cc5b1aebffd9", $"Unhandled exception when querying database. Failed to add User of Name [{addUserDto.Username}] to storage. Exception message [{ex.Message}].", ex);
            }
        }

        /// <inheritdoc />
        public async Task<UpdateUserResponseDto> UpdateUserAsync(UpdateUserRequestDto updateUserDto)
        {
            if (updateUserDto == null)
                throw new ArgumentNullException(nameof(updateUserDto));

            long requestLastRevision = updateUserDto.Revision;

            var utcNow = DateTime.UtcNow;
            updateUserDto.LastModifiedAtUtc = utcNow;
            updateUserDto.Revision++;

            try
            {
                var userItemsResult = await dynamoDBClient.PutItemAsync(new PutItemRequest
                {
                    TableName = userTableName,
                    Item = updateUserDto.ToDocument(null).ToAttributeMap(),
                    ConditionExpression = "#userId = :userId AND #revision = :revision",
                    ExpressionAttributeNames = new Dictionary<string, string>
                    {
                        { "#revision", "revision" },
                        { "#userId", "userId" },

                    },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":revision", new AttributeValue { N = requestLastRevision.ToString() } },
                        { ":userId", new AttributeValue { S = updateUserDto.Id } },
                    },
                }).ConfigureAwait(false);

                if (userItemsResult == null)
                    return null; // Item not found

                var serializedData = JsonCommonSerializer.SerializeToString(updateUserDto);
                var userDto = JsonCommonSerializer.DeserializeFromString<UpdateUserResponseDto>(serializedData);

                return userDto;
            } catch (ConditionalCheckFailedException)
            {
                throw new CommonException("39311ac1-d9fe-4580-8f2e-ba864b84f56b", $"Revision didn't match. User [{updateUserDto.Username}] won't be updated.");
            } catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(NB_MS_TO_DELAY_AFTER_PROVISION_EXCEPTION);

                throw;
            } catch (Exception ex)
            {
                // wrap exception
                throw new CommonException("18cbebf4-2184-40d6-ace8-badf1a9ed24f", $"Unhandled exception when querying database. Failed to update User [{updateUserDto.Id}] in storage. Exception message [{ex.Message}].", ex);
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            try
            {
                var response = await dynamoDBClient.DeleteItemAsync(new DeleteItemRequest
                {
                    TableName = userTableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "userId", new AttributeValue { S = userId } },
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
                throw new CommonException("078f275d-1676-4e98-ab8f-7f6d8c533a79", $"Unhandled exception when querying database. Failed to delete User matching UserId [{userId}] from storage. Exception message [{ex.Message}].", ex);
            }
        }
    }
}
