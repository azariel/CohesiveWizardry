using System.Text.Json.Serialization;

namespace CohesiveWizardry.Common.Configuration.Context
{
    public class AIContextSettings
    {
        [JsonPropertyName("systemDirectiveContext")]
        public SystemDirectiveSettings SystemDirective { get; set; }
    }
}
