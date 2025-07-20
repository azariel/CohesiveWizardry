using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest
{
    public class AddInferenceRequestWorkflow : IAddInferenceRequestWorkflow
    {
        private IInferenceRequestsDal inferenceRequestsDal = null;

        public AddInferenceRequestWorkflow(
            IInferenceRequestsDal inferenceRequestsDal)
        {
            this.inferenceRequestsDal = inferenceRequestsDal;
        }

        public async Task<object> ExecuteAsync(AddInferenceRequestDto dto)
        {
            LoggingManager.LogToFile($"11b6b900-c086-4515-972d-856377ebff0f", $"Adding new Inference Request [{dto?.Id}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            // Validate
            if (string.IsNullOrWhiteSpace(dto?.Id))
                throw new BadRequestWebApiException("9e307ac3-03af-45dd-8ae6-310cb45e2f6f", $"Invalid Dto. Id is invalid. Request payload was incorrect.");

            // Get request from storage to check if it already exists
            var inferenceRequest = await inferenceRequestsDal.GetInferenceRequestAsync(dto.Id);

            if(inferenceRequest != null)
                throw new ConflictWebApiException("40e62fd2-051c-4cbd-8703-4ff156119e8b", $"Inference Request with Id [{dto.Id}] to add already exist in the storage.");

            // Add the new inference Request
            var inferenceRequestResult = await inferenceRequestsDal.AddInferenceRequestAsync(dto);

            LoggingManager.LogToFile($"cf9ce761-93a7-41a5-aca3-eafc50a9c71f", $"New Inference Request with Id [{dto.Id}] was added.", logVerbosity: LoggingManager.LogVerbosity.Verbose);
            return inferenceRequestResult;
        }
    }
}
