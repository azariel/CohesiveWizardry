using CohesiveWizardry.Storage.Dtos.Requests.InferenceRequests;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions
{
    public interface IUpdateInferenceRequestWorkflow
    {
        Task<object> ExecuteAsync(UpdateInferenceRequestDto dto);
    }
}
