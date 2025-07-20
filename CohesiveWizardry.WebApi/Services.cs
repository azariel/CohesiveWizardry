using CohesiveWizardry.Storage.WebApi.Workflows;
using CohesiveWizardry.WebApi.Workflows;

namespace CohesiveWizardry.WebApi
{
    /// <summary>
    /// Internal DI
    /// </summary>
    internal static class Services
    {
        internal static void ConfigureServices(IServiceCollection services)
        {
            // Workflows
            services.AddScoped<IMainAddAIReplyRequestWorkflow, MainAddAIReplyRequestWorkflow>();
            services.AddScoped<IMainGetAIReplyRequestWorkflow, MainGetAIReplyRequestWorkflow>();
            services.AddScoped<IMainUpdateAIReplyRequestWorkflow, MainUpdateAIReplyRequestWorkflow>();
            services.AddScoped<IMainDeleteAIReplyRequestWorkflow, MainDeleteAIReplyRequestWorkflow>();
        }
    }
}
