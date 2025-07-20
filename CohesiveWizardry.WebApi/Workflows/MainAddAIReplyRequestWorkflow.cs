using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.WebApi.Workflows;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class MainAddAIReplyRequestWorkflow : IMainAddAIReplyRequestWorkflow
    {
        public async Task<object> ExecuteAsync(AddAIReplyRequestDto dto)
        {
            return true;
        }
    }
}
