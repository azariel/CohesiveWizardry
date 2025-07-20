using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.Dtos.Requests.Conversations;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Conversations;
using CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class GetConversationWorkflow : IGetConversationWorkflow
    {
        private IConversationsDal conversationsDal = null;

        public GetConversationWorkflow(
            IConversationsDal conversationsDal)
        {
            this.conversationsDal = conversationsDal;
        }

        public async Task<object> ExecuteAsync(GetConversationRequestDto getConversationDto)
        {
            LoggingManager.LogToFile($"eb241098-9ef8-4b8f-9e97-8881d75a5970", $"Getting Conversation with Id [{getConversationDto?.ConversationId}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (getConversationDto?.ConversationId == null)
            {
                throw new BadRequestWebApiException("66f46628-da5c-48c3-a17c-6afd82dfadd0", $"Invalid Dto. ConversationId [{getConversationDto?.ConversationId}] was invalid. Request payload was incorrect.");
            }

            // Get Conversation from storage to check if it already exists
            var response = await conversationsDal.GetConversationAsync(getConversationDto.ConversationId);
            LoggingManager.LogToFile($"3e18baab-8314-43d3-9177-4e5c1c123787", $"Conversation with Id [{getConversationDto?.ConversationId}] was Get.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            return response;
        }
    }
}
