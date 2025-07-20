using Cohesive_rp_storage_dtos.Requests.Users;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions
{
    public interface IInterfaceAddAIReplyRequestWorkflow
    {
        Task<object> ExecuteAsync(AddAIReplyRequestDto dto);
    }
}
