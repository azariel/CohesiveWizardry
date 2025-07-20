using System.Text.Json.Serialization;

namespace CohesiveWizardry.Common.Configuration
{
    public class StorageSettings
    {
        [JsonPropertyName("apiUrl")]
        public string ApiUrl { get; set; }
    }
}
