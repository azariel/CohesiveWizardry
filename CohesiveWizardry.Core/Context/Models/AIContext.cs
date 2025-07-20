using System.Text.Json.Serialization;

namespace CohesiveWizardry.Core.Context.Models
{
    /// <summary>
    /// Model representing all the context the AI has or could have.
    /// </summary>
    public class AIContext
    {
        /*
         * [System-Directive] (sectionHeader)
         * XXX (content)
         * */

        [JsonPropertyName("systemDirectiveContext")]
        public SystemDirectiveContext SystemDirective { get; set; } = new();
    }
}
