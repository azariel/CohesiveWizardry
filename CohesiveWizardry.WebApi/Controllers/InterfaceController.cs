using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CohesiveWizardry.Storage.WebApi.Controllers
{
    /// <summary>
    /// Controller around the Api offering interactions between the User and the Backend
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InterfaceController : Controller
    {
        private IInterfaceAddAIReplyRequestWorkflow mainAddAIReplyRequestWorkflow;
        private IInterfaceGetAIReplyRequestWorkflow mainGetAIReplyRequestWorkflow;
        private IInterfaceUpdateAIReplyRequestWorkflow mainUpdateAIReplyRequestWorkflow;
        private IInterfaceDeleteAIReplyRequestWorkflow mainDeleteAIReplyRequestWorkflow;

        public InterfaceController(
            IInterfaceAddAIReplyRequestWorkflow mainAddAIReplyRequestWorkflow,
            IInterfaceGetAIReplyRequestWorkflow mainGetAIReplyRequestWorkflow,
            IInterfaceUpdateAIReplyRequestWorkflow mainUpdateAIReplyRequestWorkflow,
            IInterfaceDeleteAIReplyRequestWorkflow mainDeleteAIReplyRequestWorkflow)
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
        /// Add new request to generate an AI reply.
        /// </summary>
        [HttpPost]
        [Route("AIReplyRequest")]
        public async Task<ActionResult<object>> AddAIReplyRequest([FromBody] AddAIReplyRequestDto aiReplyRequest)
        {
            object response = await mainAddAIReplyRequestWorkflow.ExecuteAsync(aiReplyRequest);
            return response;
        }

        /// <summary>
        /// Get existing request to generate an AI reply.
        /// </summary>
        [HttpGet]
        [Route("AIReplyRequest/{requestId}")]
        public async Task<ActionResult<object>> GetAIReply(GetAIReplyRequestDto aiReplyRequest)
        {
            object response = await mainGetAIReplyRequestWorkflow.ExecuteAsync(aiReplyRequest);

            if (response == null)
                return NotFound();

            return response;
        }

        /// <summary>
        /// Update (full obj) AIReplyRequest to generate an AI reply.
        /// </summary>
        [HttpPut]
        [Route("AIReplyRequest/{aiReplyIdRequestToUpdate}")]
        public async Task<ActionResult<object>> UpdateAIReplyRequest([FromRoute]string aiReplyRequestIdToUpdate, UpdateAIReplyRequestDto aiReplyRequest)
        {
            // Validate request
            if(aiReplyRequest.Id != aiReplyRequestIdToUpdate)
                throw new BadRequestWebApiException("2888a7b7-6ae6-4fe1-bad3-8a7caeb74ccd", $"AIReplyRequestId [{aiReplyRequestIdToUpdate}] to update didn't match the provided body AIReplyRequestId [{aiReplyRequest.Id}].");

            object response = await mainUpdateAIReplyRequestWorkflow.ExecuteAsync(aiReplyRequest);
            return response;
        }

        /// <summary>
        /// Delete AIReplyRequest to generate an AI reply.
        /// </summary>
        [HttpDelete]
        [Route("AIReplyRequest/{aiReplyRequestId}")]
        public async Task<ActionResult<object>> DeleteAIReplyRequest(DeleteAIReplyRequestDto aiReplyRequest)
        {
            object response = await mainDeleteAIReplyRequestWorkflow.ExecuteAsync(aiReplyRequest);
            return response;
        }
    }
}
