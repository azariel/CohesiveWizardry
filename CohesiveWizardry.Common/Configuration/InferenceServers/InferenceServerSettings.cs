using System.Text.Json.Serialization;

namespace CohesiveWizardry.Common.Configuration.InferenceServers
{
    /// <summary>
    /// Configuration around the different available inference servers
    /// </summary>
    public class InferenceServerSettings
    {
        [JsonPropertyName("inferenceServerActionTypes")]
        public List<InferenceServerActionType> InferenceServerActionTypes { get; set; } = new();

        [JsonPropertyName("webApiUrl")]
        public string WebApiUrl { get; set; } = null;
    }
}
