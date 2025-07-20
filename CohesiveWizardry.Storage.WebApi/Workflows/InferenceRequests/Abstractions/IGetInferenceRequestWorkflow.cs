using CohesiveWizardry.Storage.Dtos.Requests.InferenceRequests;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions
{
    public interface IGetInferenceRequestWorkflow
    {
        Task<object> ExecuteAsync(GetInferenceRequestDto dto);
    }
}
