using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.WebApi.Workflows;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class MainUpdateAIReplyRequestWorkflow : IMainUpdateAIReplyRequestWorkflow
    {
        public async Task<object> ExecuteAsync(UpdateAIReplyRequestDto dto)
        {
            return true;
        }
    }
}
