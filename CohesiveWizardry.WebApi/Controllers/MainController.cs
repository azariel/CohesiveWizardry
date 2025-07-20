using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.WebApi.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace CohesiveWizardry.Storage.WebApi.Controllers
{
    /// <summary>
    /// Controller around the Main Api
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : Controller
    {
        private IMainAddAIReplyRequestWorkflow mainAddAIReplyRequestWorkflow;
        private IMainGetAIReplyRequestWorkflow mainGetAIReplyRequestWorkflow;
        private IMainUpdateAIReplyRequestWorkflow mainUpdateAIReplyRequestWorkflow;
        private IMainDeleteAIReplyRequestWorkflow mainDeleteAIReplyRequestWorkflow;

        public MainController(
            IMainAddAIReplyRequestWorkflow mainAddAIReplyRequestWorkflow,
            IMainGetAIReplyRequestWorkflow mainGetAIReplyRequestWorkflow,
            IMainUpdateAIReplyRequestWorkflow mainUpdateAIReplyRequestWorkflow,
            IMainDeleteAIReplyRequestWorkflow mainDeleteAIReplyRequestWorkflow)
        {
            this.mainAddAIReplyRequestWorkflow = mainAddAIReplyRequestWorkflow;
            this.mainGetAIReplyRequestWorkflow = mainGetAIReplyRequestWorkflow;
            this.mainUpdateAIReplyRequestWorkflow = mainUpdateAIReplyRequestWorkflow;
            this.mainDeleteAIReplyRequestWorkflow = mainDeleteAIReplyRequestWorkflow;
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
        /// Add new request to memory.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> AddAIReplyRequest([FromBody] AddAIReplyRequestDto aiReplyRequest)
        {
            object response = await mainAddAIReplyRequestWorkflow.ExecuteAsync(aiReplyRequest);
            return response;
        }

        /// <summary>
        /// Get existing request from memory.
        /// </summary>
        [HttpGet]
        [Route("{requestId}")]
        public async Task<ActionResult<object>> GetAIReply(GetAIReplyRequestDto aiReplyRequest)
        {
            object response = await mainGetAIReplyRequestWorkflow.ExecuteAsync(aiReplyRequest);

            if (response == null)
                return NotFound();

            return response;
        }

        /// <summary>
        /// Update (full obj) AIReplyRequest to memory.
        /// </summary>
        [HttpPut]
        [Route("{aiReplyIdRequestToUpdate}")]
        public async Task<ActionResult<object>> UpdateAIReplyRequest([FromRoute]string aiReplyRequestIdToUpdate, UpdateAIReplyRequestDto aiReplyRequest)
        {
            // Validate request
            if(aiReplyRequest.Id != aiReplyRequestIdToUpdate)
                throw new BadRequestWebApiException("7c6bf066-461d-457b-86fa-af0b1cf6a6bc", $"AIReplyRequestId [{aiReplyRequestIdToUpdate}] to update didn't match the provided body AIReplyRequestId [{aiReplyRequest.Id}].");

            object response = await mainUpdateAIReplyRequestWorkflow.ExecuteAsync(aiReplyRequest);
            return response;
        }

        /// <summary>
        /// Delete AIReplyRequest from memory.
        /// </summary>
        [HttpDelete]
        [Route("{aiReplyRequestId}")]
        public async Task<ActionResult<object>> DeleteAIReplyRequest(DeleteAIReplyRequestDto aiReplyRequest)
        {
            object response = await mainDeleteAIReplyRequestWorkflow.ExecuteAsync(aiReplyRequest);
            return response;
        }
    }
}
