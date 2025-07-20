using CohesiveWizardry.Storage.Dtos.Requests.Conversations;

namespace CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions
{
    public interface IAddConversationWorkflow
    {
        Task<object> ExecuteAsync(AddConversationRequestDto dto);
    }
}
