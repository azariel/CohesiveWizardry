using System.Text.Json;
using CohesiveWizardry.Common.Diagnostics;

namespace CohesiveWizardry.Common.Serialization
{
    public static class JsonCommonSerializer
    {
        // ********************************************************************
        //                            Public
        // ********************************************************************
        public static T DeserializeFromString<T>(string serializedObjectToDeserialize)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(serializedObjectToDeserialize, Settings);
            } catch (Exception ex)
            {
                LoggingManager.LogToFile("46d3c745-1261-4ef1-8e30-ec554d0dfe44", $"Couldn't deserialize object [{typeof(T)}]", ex);
                throw;
            }
        }

        public static string SerializeToString<T>(T objectToSerialize)
        {
            try
            {
                return JsonSerializer.Serialize(objectToSerialize, Settings);
            } catch (Exception ex)
            {
                LoggingManager.LogToFile("49811773-4a9a-4500-8109-8de4226ddc4b", $"Couldn't serialize object [{typeof(T)}]", ex);
                throw;
            }
        }

        // ********************************************************************
        //                            Properties
        // ********************************************************************
        public static JsonSerializerOptions Settings => new()
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };
    }
}
