using Cohesive_rp_storage_dtos.Requests.Users;

namespace CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions
{
    public interface IAddUserRequestWorkflow
    {
        Task<object> ExecuteAsync(AddUserRequestDto dto);
    }
}
