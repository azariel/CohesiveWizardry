using System.Text.Json.Serialization;

namespace CohesiveWizardry.Core.Context.Models
{
    public class SystemDirectiveContext
    {
        [JsonPropertyName("header")]
        public string SectionHeader { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
