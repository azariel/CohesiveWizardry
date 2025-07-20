using System.Text.Json.Serialization;

namespace CohesiveWizardry.Common.Configuration.Context
{
    public class SystemDirectiveSettings
    {
        [JsonPropertyName("header")]
        public string Header { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
