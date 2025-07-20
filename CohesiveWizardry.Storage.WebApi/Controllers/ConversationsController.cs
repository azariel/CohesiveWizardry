using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.Dtos.Requests.Conversations;
using CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CohesiveWizardry.Storage.WebApi.Controllers
{
    /// <summary>
    /// Controller around conversation. A conversation is a wrapper of messages between the AI and a User. A full Context is tied to the conversation.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : Controller
    {
        private IAddConversationWorkflow addConversationWorkflow;
        private IGetConversationWorkflow getConversationWorkflow;
        private IUpdateConversationWorkflow updateConversationWorkflow;
        private IDeleteConversationWorkflow deleteConversationWorkflow;

        public ConversationsController(
            IAddConversationWorkflow addConversationWorkflow,
            IGetConversationWorkflow getConversationWorkflow,
            IUpdateConversationWorkflow updateConversationWorkflow,
            IDeleteConversationWorkflow deleteConversationWorkflow)
        {
            this.addConversationWorkflow = addConversationWorkflow;
            this.getConversationWorkflow = getConversationWorkflow;
            this.updateConversationWorkflow = updateConversationWorkflow;
            this.deleteConversationWorkflow = deleteConversationWorkflow;
        }

        /// <summary>
        /// Just a ping-like endpoint.
        /// </summary>
        [HttpGet]
        [Route("teapot")]
        public async Task<ActionResult<object>> Teapot()
        {
            return "I am a teapot";
        }

        /// <summary>
        /// Get conversation from storage.
        /// </summary>
        [HttpGet]
        [Route("{conversationId}")]
        public async Task<ActionResult<object>> GetConversation(GetConversationRequestDto conversationDto)
        {
            object response = await getConversationWorkflow.ExecuteAsync(conversationDto);

            if (response == null)
                return NotFound();

            return response;
        }

        /// <summary>
        /// Add new conversation to storage.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> AddConversation([FromBody] AddConversationRequestDto conversationDto)
        {
            object response = await addConversationWorkflow.ExecuteAsync(conversationDto);
            return response;
        }

        /// <summary>
        /// Update (full obj) conversation to storage.
        /// </summary>
        [HttpPut]
        [Route("{conversationIdToUpdate}")]
        public async Task<ActionResult<object>> UpdateConversation([FromRoute]string conversationIdToUpdate, UpdateConversationRequestDto conversationDto)
        {
            // Validate request
            if(conversationDto.Id != conversationIdToUpdate)
                throw new BadRequestWebApiException("44df4a23-c94f-4bc6-929b-ec69c0dd98e0", $"Conversation Id [{conversationIdToUpdate}] to update didn't match the provided body Conversation Id [{conversationDto.Id}].");

            object response = await updateConversationWorkflow.ExecuteAsync(conversationDto);
            return response;
        }

        /// <summary>
        /// Delete Conversation from storage.
        /// </summary>
        [HttpDelete]
        [Route("{conversationId}")]
        public async Task<ActionResult<object>> DeleteConversation(DeleteConversationRequestDto conversationDto)
        {
            object response = await deleteConversationWorkflow.ExecuteAsync(conversationDto);
            return response;
        }
    }
}
