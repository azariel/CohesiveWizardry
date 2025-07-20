using CohesiveWizardry.Storage.Dtos.Requests.InferenceRequests;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions
{
    public interface IDeleteInferenceRequestWorkflow
    {
        Task<object> ExecuteAsync(DeleteInferenceRequestDto dto);
    }
}
