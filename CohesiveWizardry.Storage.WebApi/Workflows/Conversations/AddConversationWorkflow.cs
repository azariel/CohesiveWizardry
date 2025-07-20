using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.Dtos.Requests.Conversations;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Conversations;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class AddConversationWorkflow : IAddConversationWorkflow
    {
        private IConversationsDal conversationsDal = null;
        private IUsersDal usersDal = null;

        public AddConversationWorkflow(
            IConversationsDal conversationsDal,
            IUsersDal usersDal)
        {
            this.conversationsDal = conversationsDal;
            this.usersDal = usersDal;
        }

        public async Task<object> ExecuteAsync(AddConversationRequestDto addConversationDto)
        {
            LoggingManager.LogToFile($"225bdc3d-3dee-4510-aca9-c63dc8665174", $"Adding new Conversation [{addConversationDto?.Id}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (string.IsNullOrWhiteSpace(addConversationDto?.Id))
                throw new BadRequestWebApiException("944a025f-9c02-4af8-9f57-6e34a8ee3db7", $"Invalid Dto. Id is invalid. Request payload was incorrect.");

            if (string.IsNullOrWhiteSpace(addConversationDto?.UserId))
                throw new BadRequestWebApiException("0af6b6b3-a3ff-40e5-a56f-f8a9ca952cb1", $"Invalid Dto. UserId [{addConversationDto?.UserId}] was invalid. Request payload was incorrect.");

            // Get Conversation from storage to check if it already exists
            var conversation = await conversationsDal.GetConversationAsync(addConversationDto.Id);

            if(conversation != null)
                throw new ConflictWebApiException("99bc43fd-7453-4dcb-8d9a-5bc38c26a549", $"Conversation with Id [{addConversationDto.Id}] to add already exist in the storage.");

            // Get the User to link to this conversation as the User must exists
            var user = await usersDal.GetUserAsync(addConversationDto.UserId);

            if(user == null)
                throw new BadRequestWebApiException("27d2e1b3-3e3a-401f-9f68-09e210f73244", $"UserId [{addConversationDto.UserId}] does not exist in the storage. The conversation could not be added.");

            // Add the new conversation
            var conversationResult = await conversationsDal.AddConversationAsync(addConversationDto);

            LoggingManager.LogToFile($"df809a54-b039-4efc-84ba-e0077cafa4af", $"New Conversation [{addConversationDto?.Id}] with Id [{addConversationDto?.Id}] was added.", logVerbosity: LoggingManager.LogVerbosity.Verbose);
            return conversationResult;
        }
    }
}
