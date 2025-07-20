using System.Text;
using CohesiveWizardry.Core.Context.Models;

namespace CohesiveWizardry.Core.Prompt
{
    public static class PromptBuilder
    {
        public static string BuildPromptFromAIContext(AIContext aiContext)
        {
            if(aiContext == null)
                return string.Empty;

            StringBuilder promptStrBuilder = new();

            // System Directive handling
            AddSystemDirectiveContext(aiContext, promptStrBuilder);

            // TODO: handle other info
            return promptStrBuilder.ToString();
        }

        private static void AddSystemDirectiveContext(AIContext aiContext, StringBuilder promptStrBuilder)
        {
            promptStrBuilder.AppendLine(aiContext.SystemDirective.SectionHeader);
            promptStrBuilder.AppendLine(aiContext.SystemDirective.Content);
        }
    }
}
