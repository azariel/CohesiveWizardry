using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CohesiveWizardry.Storage.WebApi.Controllers
{
    /// <summary>
    /// Controller around Inference Requests. An inference request is a request to run against a backend LLM inference server.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InferenceRequestsController : Controller
    {
        private IAddInferenceRequestWorkflow addInferenceRequestWorkflow;
        private IGetInferenceRequestWorkflow getInferenceRequestWorkflow;
        private IUpdateInferenceRequestWorkflow updateInferenceRequestWorkflow;
        private IDeleteInferenceRequestWorkflow deleteInferenceRequestWorkflow;

        public InferenceRequestsController(
            IAddInferenceRequestWorkflow addInferenceRequestWorkflow,
            IGetInferenceRequestWorkflow getInferenceRequestWorkflow,
            IUpdateInferenceRequestWorkflow updateInferenceRequestWorkflow,
            IDeleteInferenceRequestWorkflow deleteInferenceRequestWorkflow)
        {
            this.addInferenceRequestWorkflow = addInferenceRequestWorkflow;
            this.getInferenceRequestWorkflow = getInferenceRequestWorkflow;
            this.updateInferenceRequestWorkflow = updateInferenceRequestWorkflow;
            this.deleteInferenceRequestWorkflow = deleteInferenceRequestWorkflow;
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
        /// Get new Inference from storage.
        /// </summary>
        [HttpGet]
        [Route("{inferenceRequestId}")]
        public async Task<ActionResult<object>> GetInference(GetInferenceRequestDto inferenceRequest)
        {
            object response = await getInferenceRequestWorkflow.ExecuteAsync(inferenceRequest);

            if (response == null)
                return NotFound();

            return response;
        }

        /// <summary>
        /// Add new Inference to storage.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> AddInference([FromBody] AddInferenceRequestDto inferenceRequest)
        {
            object response = await addInferenceRequestWorkflow.ExecuteAsync(inferenceRequest);
            return response;
        }

        /// <summary>
        /// Update (full obj) Inference to storage.
        /// </summary>
        [HttpPut]
        [Route("{InferenceIdToUpdate}")]
        public async Task<ActionResult<object>> UpdateInference([FromRoute]string inferenceIdToUpdate, UpdateInferenceRequestDto inferenceRequest)
        {
            // Validate request
            if(inferenceRequest.Id != inferenceIdToUpdate)
                throw new BadRequestWebApiException("f3924c22-07dd-4a0c-9600-4e9e206d163c", $"InferenceId [{inferenceIdToUpdate}] to update didn't match the provided body InferenceId [{inferenceRequest.Id}].");

            object response = await updateInferenceRequestWorkflow.ExecuteAsync(inferenceRequest);
            return response;
        }

        /// <summary>
        /// Delete Inference from storage.
        /// </summary>
        [HttpDelete]
        [Route("{inferenceRequestId}")]
        public async Task<ActionResult<object>> DeleteInference(DeleteInferenceRequestDto inferenceRequest)
        {
            object response = await deleteInferenceRequestWorkflow.ExecuteAsync(inferenceRequest);
            return response;
        }
    }
}
