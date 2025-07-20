using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.Workflows;
using CohesiveWizardry.Storage.WebApi.Workflows.Users;

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
            services.AddScoped<IAddUserRequestWorkflow, AddUserRequestWorkflow>();
            services.AddScoped<IGetUserRequestWorkflow, GetUserRequestWorkflow>();
            services.AddScoped<IUpdateUserRequestWorkflow, UpdateUserRequestWorkflow>();
            services.AddScoped<IDeleteUserRequestWorkflow, DeleteUserRequestWorkflow>();

            // Dals
            services.AddScoped<IUsersDal, UsersDal>();
        }
    }
}
