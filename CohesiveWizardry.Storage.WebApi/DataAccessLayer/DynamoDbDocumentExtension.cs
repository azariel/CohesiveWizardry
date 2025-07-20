using System.Text.Json;
using Amazon.DynamoDBv2.DocumentModel;
using Cohesive_rp_storage_dtos;
using CohesiveWizardry.Storage.Dtos.Requests;

namespace CohesiveWizardry.Storage.WebApi.DataAccessLayer
{
    /// <summary>
    /// Extensions to convert a model to a DB Document.
    /// </summary>
    public static class DynamoDbDocumentExtension
    {
        /// <summary>
        /// Converts the actual HistoryData to Document config instance for DynamoDB.
        /// </summary>
        /// <param name="dataItem">The instance of HistoryData.</param>
        /// <returns>The instance of Document for storing in DynamoDB.</returns>
        public static Document ToDocument(this IStorageDto dataItem, JsonSerializerOptions serializerOptions = null)
        {
            if (dataItem == null)
                throw new ArgumentNullException(nameof(dataItem));

            // Note that serializing directly the dataItem using Json.Text doesn't serialize child properties, which was working with newtonsoft. The following code fix that behavior.
            string serializedValue = dataItem switch
            {
                UserDto userDto => JsonSerializer.Serialize(userDto, serializerOptions),
                InferenceRequestDto inferenceRequestDto => JsonSerializer.Serialize(inferenceRequestDto, serializerOptions),
                _ => throw new Exception($"Type {dataItem.GetType()} is unhandled."),
            };

            var document = Document.FromJson(serializedValue);
            return document;
        }
    }
}
