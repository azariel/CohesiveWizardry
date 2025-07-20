using CohesiveWizardry.Storage.Dtos.Requests.Users;

namespace CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions
{
    public interface IDeleteUserRequestWorkflow
    {
        Task<object> ExecuteAsync(DeleteUserRequestDto dto);
    }
}
