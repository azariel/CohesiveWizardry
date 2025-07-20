using Cohesive_rp_storage_dtos.Requests.Users;

namespace CohesiveWizardry.Storage.WebApi.Workflows.Users
{
    public interface IUpdateUserRequestWorkflow
    {
        Task<object> ExecuteAsync(UpdateUserRequestDto dto);
    }
}
