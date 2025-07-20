using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.WebApi.Workflows;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class MainDeleteAIReplyRequestWorkflow : IMainDeleteAIReplyRequestWorkflow
    {
        public async Task<object> ExecuteAsync(DeleteAIReplyRequestDto dto)
        {
            return true;
        }
    }
}
