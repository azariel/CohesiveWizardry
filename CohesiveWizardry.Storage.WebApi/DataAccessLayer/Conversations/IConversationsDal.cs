using CohesiveWizardry.Storage.Dtos.Requests.Conversations;
using CohesiveWizardry.Storage.Dtos.Responses.Conversations;

namespace CohesiveWizardry.Storage.WebApi.DataAccessLayer.Conversations
{
    public interface IConversationsDal
    {
        Task<GetConversationResponseDto> GetConversationAsync(string conversationId);
        Task<AddConversationResponseDto> AddConversationAsync(AddConversationRequestDto addConversationDto);
        Task<UpdateConversationResponseDto> UpdateConversationAsync(UpdateConversationRequestDto updateConversationDto);
        Task<bool> DeleteConversationAsync(string conversationId);
    }
}
