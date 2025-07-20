using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.Dtos.Requests.Conversations;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Conversations;
using CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class DeleteConversationWorkflow : IDeleteConversationWorkflow
    {
        private IConversationsDal conversationsDal = null;

        public DeleteConversationWorkflow(
            IConversationsDal conversationsDal)
        {
            this.conversationsDal = conversationsDal;
        }

        public async Task<object> ExecuteAsync(DeleteConversationRequestDto deleteConversationDto)
        {
            LoggingManager.LogToFile($"eb67eda3-d79f-4bef-bfc1-43665e4b44d5", $"Deleting Conversation with Id [{deleteConversationDto?.ConversationId}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (deleteConversationDto?.ConversationId == null)
            {
                throw new BadRequestWebApiException("7b3d529f-85a7-4e7d-a2c3-42bb13225b15", $"Invalid Dto. ConversationId [{deleteConversationDto?.ConversationId}] was invalid. Request payload was incorrect.");
            }

            // Get Conversation from storage to check if it exists
            var conversation = await conversationsDal.GetConversationAsync(deleteConversationDto.ConversationId);

            if(conversation == null)
            {
                throw new BadRequestWebApiException("942aa01c-f1eb-439a-80a2-70130740d8e9", $"ConversationId [{deleteConversationDto.ConversationId}] to delete didn't exist in the storage.");
            }

            bool result = await conversationsDal.DeleteConversationAsync(deleteConversationDto.ConversationId);
            LoggingManager.LogToFile($"a16f1c76-1ddc-4a2f-84b6-b2672fe131b8", $"Conversation with Id [{deleteConversationDto?.ConversationId}] was removed.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            return result;
        }
    }
}
