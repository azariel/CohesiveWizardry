using Cohesive_rp_storage_dtos.Requests.Users;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions
{
    public interface IGetInferenceRequestWorkflow
    {
        Task<object> ExecuteAsync(GetInferenceRequestDto dto);
    }
}
