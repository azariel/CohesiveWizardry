using CohesiveWizardry.Common.Configuration;
using CohesiveWizardry.Core.Context.Models;

namespace CohesiveWizardry.Core.Context
{
    public static class AIContextBuilder
    {
        public static AIContext BuildContext()
        {
            var config = CommonConfigurationManager.GetConfigFromMemory();
            var aiContext = new AIContext();

            // The System Directive is configurable and is stored in the applicative config
            aiContext.SystemDirective.SectionHeader = config.AIContextSettings.SystemDirective.Header;
            aiContext.SystemDirective.Content = config.AIContextSettings.SystemDirective.Content;

            // TODO: fetch all the context modules here
            return aiContext;
        }
    }
}
