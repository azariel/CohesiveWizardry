using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.Dtos.Requests.Conversations;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Conversations;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class UpdateConversationWorkflow : IUpdateConversationWorkflow
    {
        private IConversationsDal conversationsDal = null;
        private IUsersDal usersDal = null;

        public UpdateConversationWorkflow(
            IConversationsDal conversationsDal,
            IUsersDal usersDal)
        {
            this.conversationsDal = conversationsDal;
            this.usersDal = usersDal;
        }

        public async Task<object> ExecuteAsync(UpdateConversationRequestDto updateConversationDto)
        {
            LoggingManager.LogToFile($"b1e42974-ce17-49ef-9f53-2db0528ad173", $"Updating Conversation with Id [{updateConversationDto?.Id}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (updateConversationDto?.Id == null)
            {
                throw new BadRequestWebApiException("70c66806-7997-478d-abc7-eae8e9e92d75", $"Invalid Dto. ConversationId [{updateConversationDto?.Id}] was invalid. Request payload was incorrect.");
            }

            // Get Conversation from storage to check if it already exists
            var conversation = await conversationsDal.GetConversationAsync(updateConversationDto.Id);

            if (conversation == null)
                throw new ConflictWebApiException("867a758a-5f2c-439e-bf12-25d1fe0fba47", $"Can't update Conversation with Id [{conversation.Id}] does not exist in storage.");

            // Get the User to link to this conversation as the User must exists
            var user = await usersDal.GetUserAsync(updateConversationDto.UserId);

            if(user == null)
                throw new BadRequestWebApiException("27d2e1b3-3e3a-401f-9f68-09e210f73244", $"UserId [{updateConversationDto.UserId}] does not exist in the storage. The conversation could not be added.");

            // Update the new conversation
            var conversationResult = await conversationsDal.UpdateConversationAsync(updateConversationDto);
            LoggingManager.LogToFile($"a0bc82e8-2ac7-4ad2-8396-4cfbd66fde07", $"Conversation with Id [{updateConversationDto?.Id}] was updated.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            return conversationResult;
        }
    }
}
