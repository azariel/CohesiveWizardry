using CohesiveWizardry.Storage.WebApi.DataAccessLayer.InferenceRequests;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.Workflows;
using CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

namespace CohesiveWizardry.Storage.WebApi
{
    /// <summary>
    /// Internal DI
    /// </summary>
    internal static class Services
    {
        internal static void ConfigureServices(IServiceCollection services)
        {
            // Workflows
            // --Users--
            services.AddScoped<IAddUserRequestWorkflow, AddUserRequestWorkflow>();
            services.AddScoped<IGetUserRequestWorkflow, GetUserRequestWorkflow>();
            services.AddScoped<IUpdateUserRequestWorkflow, UpdateUserRequestWorkflow>();
            services.AddScoped<IDeleteUserRequestWorkflow, DeleteUserRequestWorkflow>();

            // --InferenceRequests--
            services.AddScoped<IAddInferenceRequestWorkflow, AddInferenceRequestWorkflow>();
            services.AddScoped<IGetInferenceRequestWorkflow, GetInferenceRequestWorkflow>();
            services.AddScoped<IUpdateInferenceRequestWorkflow, UpdateInferenceRequestWorkflow>();
            services.AddScoped<IDeleteInferenceRequestWorkflow, DeleteInferenceRequestWorkflow>();

            // Dals
            services.AddScoped<IUsersDal, UsersDal>();
            services.AddScoped<IInferenceRequestsDal, InferenceRequestsDal>();
        }
    }
}
