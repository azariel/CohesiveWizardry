using CohesiveWizardry.Storage.Dtos.Requests.Users;

namespace CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions
{
    public interface IUpdateUserRequestWorkflow
    {
        Task<object> ExecuteAsync(UpdateUserRequestDto dto);
    }
}
