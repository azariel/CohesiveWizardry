using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.Dtos.Requests.InferenceRequests;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest
{
    public class DeleteInferenceRequestWorkflow : IDeleteInferenceRequestWorkflow
    {
        private IInferenceRequestsDal inferenceRequestsDal = null;

        public DeleteInferenceRequestWorkflow(
            IInferenceRequestsDal inferenceRequestsDal)
        {
            this.inferenceRequestsDal = inferenceRequestsDal;
        }

        public async Task<object> ExecuteAsync(DeleteInferenceRequestDto deleteInferenceRequestDto)
        {
            LoggingManager.LogToFile($"24e568b4-a10a-4598-b6e9-4d61ebae5c05", $"Deleting InferenceRequest with Id [{deleteInferenceRequestDto?.InferenceRequestId}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (deleteInferenceRequestDto?.InferenceRequestId == null)
            {
                throw new BadRequestWebApiException("5804ed03-42f5-496b-a668-14d8f33bfbf0", $"Invalid Dto. InferenceRequestId [{deleteInferenceRequestDto?.InferenceRequestId}] was invalid. Request payload was incorrect.");
            }

            // Get InferenceRequest from storage to check if it exists
            var inferenceRequest = await inferenceRequestsDal.GetInferenceRequestAsync(deleteInferenceRequestDto.InferenceRequestId);

            if(inferenceRequest == null)
            {
                throw new BadRequestWebApiException("56cbe73a-7b3b-4f89-bbcd-6b71f8691c7b", $"InferenceRequestId [{deleteInferenceRequestDto.InferenceRequestId}] to delete didn't exist in the storage.");
            }

            bool result = await inferenceRequestsDal.DeleteInferenceRequestAsync(deleteInferenceRequestDto.InferenceRequestId);
            LoggingManager.LogToFile($"266b6cc2-7e90-4008-9163-1211f40779a7", $"InferenceRequest with Id [{deleteInferenceRequestDto?.InferenceRequestId}] was removed.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            return result;
        }
    }
}
