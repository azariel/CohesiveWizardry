using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

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
            services.AddScoped<IInterfaceAddAIReplyRequestWorkflow, InterfaceAddAIReplyRequestWorkflow>();
            services.AddScoped<IInterfaceGetAIReplyRequestWorkflow, InterfaceGetAIReplyRequestWorkflow>();
            services.AddScoped<IInterfaceUpdateAIReplyRequestWorkflow, InterfaceUpdateAIReplyRequestWorkflow>();
            services.AddScoped<IInterfaceDeleteAIReplyRequestWorkflow, InterfaceDeleteAIReplyRequestWorkflow>();
        }
    }
}
