using Cohesive_rp_storage_dtos.Requests.Users;

namespace CohesiveWizardry.WebApi.Workflows
{
    public interface IMainDeleteAIReplyRequestWorkflow
    {
        Task<object> ExecuteAsync(DeleteAIReplyRequestDto dto);
    }
}
