using CohesiveWizardry.Storage.Dtos.Requests.Users;

namespace CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions
{
    public interface IGetUserRequestWorkflow
    {
        Task<object> ExecuteAsync(GetUserRequestDto dto);
    }
}
