using System.Text.Json.Serialization;
using CohesiveWizardry.Common.Configuration.InferenceServers;
using static CohesiveWizardry.Common.Diagnostics.LoggingManager;

namespace CohesiveWizardry.Common.Configuration
{
    /// <summary>
    /// Model of the configuration embedded within the file CommonConfigurationManager.CONFIG_FILE_NAME
    /// </summary>
    public class CommonConfiguration
    {
        [JsonPropertyName("logVerbosity")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogVerbosity LogVerbosity { get; set; }

        [JsonPropertyName("inferenceServersSettings")]
        public List<InferenceServerSettings> InferenceServersSettings { get; set; }
    }
}
