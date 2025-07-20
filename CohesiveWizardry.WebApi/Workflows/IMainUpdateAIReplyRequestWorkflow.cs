using Cohesive_rp_storage_dtos.Requests.Users;

namespace CohesiveWizardry.WebApi.Workflows
{
    public interface IMainUpdateAIReplyRequestWorkflow
    {
        Task<object> ExecuteAsync(UpdateAIReplyRequestDto dto);
    }
}
