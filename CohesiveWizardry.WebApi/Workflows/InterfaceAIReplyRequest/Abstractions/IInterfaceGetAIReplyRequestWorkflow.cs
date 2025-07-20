using Cohesive_rp_storage_dtos.Requests.Users;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions
{
    public interface IInterfaceGetAIReplyRequestWorkflow
    {
        Task<object> ExecuteAsync(GetAIReplyRequestDto dto);
    }
}
