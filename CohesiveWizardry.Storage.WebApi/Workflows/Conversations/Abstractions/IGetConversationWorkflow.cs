using CohesiveWizardry.Storage.Dtos.Requests.Conversations;

namespace CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions
{
    public interface IGetConversationWorkflow
    {
        Task<object> ExecuteAsync(GetConversationRequestDto dto);
    }
}
