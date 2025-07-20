using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.Dtos.Requests.InferenceRequests;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest
{
    public class GetInferenceRequestWorkflow : IGetInferenceRequestWorkflow
    {
        private IInferenceRequestsDal inferenceRequestsDal = null;

        public GetInferenceRequestWorkflow(
            IInferenceRequestsDal inferenceRequestsDal)
        {
            this.inferenceRequestsDal = inferenceRequestsDal;
        }

        public async Task<object> ExecuteAsync(GetInferenceRequestDto getInferenceRequestDto)
        {
            LoggingManager.LogToFile($"e3a22a62-d1a7-46c7-a024-092053047bfb", $"Getting InferenceRequest with Id [{getInferenceRequestDto?.InferenceRequestId}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (getInferenceRequestDto?.InferenceRequestId == null)
            {
                throw new BadRequestWebApiException("87590262-78f5-47c4-ba7c-520cb64ce5ab", $"Invalid Dto. InferenceRequestId [{getInferenceRequestDto?.InferenceRequestId}] was invalid. Request payload was incorrect.");
            }

            // Get User from storage to check if it already exists
            var response = await inferenceRequestsDal.GetInferenceRequestAsync(getInferenceRequestDto.InferenceRequestId);
            LoggingManager.LogToFile($"417d04a5-2376-4afb-9ca4-ce154b5f3157", $"InferenceRequest with Id [{getInferenceRequestDto?.InferenceRequestId}] was Get.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            return response;
        }
    }
}
