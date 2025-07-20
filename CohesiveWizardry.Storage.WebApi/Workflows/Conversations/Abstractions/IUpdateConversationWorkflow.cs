using CohesiveWizardry.Storage.Dtos.Requests.Conversations;

namespace CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions
{
    public interface IUpdateConversationWorkflow
    {
        Task<object> ExecuteAsync(UpdateConversationRequestDto dto);
    }
}
