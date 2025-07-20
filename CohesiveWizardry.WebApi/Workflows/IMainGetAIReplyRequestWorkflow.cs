using Cohesive_rp_storage_dtos.Requests.Users;

namespace CohesiveWizardry.WebApi.Workflows
{
    public interface IMainGetAIReplyRequestWorkflow
    {
        Task<object> ExecuteAsync(GetAIReplyRequestDto dto);
    }
}
