using CohesiveWizardry.Storage.Dtos.Requests.Conversations;

namespace CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions
{
    public interface IDeleteConversationWorkflow
    {
        Task<object> ExecuteAsync(DeleteConversationRequestDto dto);
    }
}
