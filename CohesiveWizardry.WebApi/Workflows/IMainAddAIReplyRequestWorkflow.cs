using Cohesive_rp_storage_dtos.Requests.Users;

namespace CohesiveWizardry.WebApi.Workflows
{
    public interface IMainAddAIReplyRequestWorkflow
    {
        Task<object> ExecuteAsync(AddAIReplyRequestDto dto);
    }
}
