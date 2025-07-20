using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.WebApi.Workflows;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class MainGetAIReplyRequestWorkflow : IMainGetAIReplyRequestWorkflow
    {
        public async Task<object> ExecuteAsync(GetAIReplyRequestDto dto)
        {
            return true;
        }
    }
}
