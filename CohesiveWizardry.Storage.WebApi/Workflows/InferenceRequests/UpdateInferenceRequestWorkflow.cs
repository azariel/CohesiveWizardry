using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest
{
    public class UpdateInferenceRequestWorkflow : IUpdateInferenceRequestWorkflow
    {
        private IInferenceRequestsDal inferenceRequestsDal = null;

        public UpdateInferenceRequestWorkflow(
            IInferenceRequestsDal inferenceRequestsDal)
        {
            this.inferenceRequestsDal = inferenceRequestsDal;
        }

        public async Task<object> ExecuteAsync(UpdateInferenceRequestDto updateInferenceRequestDto)
        {
            LoggingManager.LogToFile($"cebe84a9-7b67-4fda-900e-333e4a45a449", $"Updating Inference Request with Id [{updateInferenceRequestDto?.Id}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (updateInferenceRequestDto?.Id == null)
            {
                throw new BadRequestWebApiException("21baeac6-1b61-495b-a87b-be75d5e71cac", $"Invalid Dto. Inference RequestId [{updateInferenceRequestDto?.Id}] was invalid. Request payload was incorrect.");
            }

            // Get Inference Request from storage to check if it already exists
            var inferenceRequest = await inferenceRequestsDal.GetInferenceRequestAsync(updateInferenceRequestDto.Id);

            if (inferenceRequest == null)
            {
                return $"Can't update inference Request. Inference Request with id [{updateInferenceRequestDto.Id}] doesn't exists.";
            }

            // Update the new inference Request
            var inferenceRequestResult = await inferenceRequestsDal.UpdateInferenceRequestAsync(updateInferenceRequestDto);
            LoggingManager.LogToFile($"f0d21cf0-fd6c-40d3-a1bd-c909239db24b", $"Inference Request with Id [{updateInferenceRequestDto?.Id}] was updated.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            return inferenceRequestResult;
        }
    }
}
