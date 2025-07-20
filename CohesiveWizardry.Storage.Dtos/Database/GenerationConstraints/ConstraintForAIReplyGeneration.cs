using System.Text.Json.Serialization;

namespace CohesiveWizardry.Storage.Dtos.Database.GenerationConstraints
{
    public class ConstraintForAIReplyGeneration
    {
        [JsonPropertyName("enforcementType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConstraintEnforcementType EnforcementType { get; set; }
    }
}
