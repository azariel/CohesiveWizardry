using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.Workflows;

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
            services.AddScoped<IUsersWorkflow, UsersWorkflow>();

            // Dals
            services.AddScoped<IUsersDal, UsersDal>();
        }
    }
}
