using Cohesive_rp_storage_dtos.Requests.Users;

namespace CohesiveWizardry.Storage.WebApi.Workflows.Users
{
    public interface IGetUserRequestWorkflow
    {
        Task<object> ExecuteAsync(GetUserRequestDto dto);
    }
}
