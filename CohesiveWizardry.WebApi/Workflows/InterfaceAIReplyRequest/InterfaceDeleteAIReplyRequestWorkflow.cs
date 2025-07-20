using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest
{
    public class InterfaceDeleteAIReplyRequestWorkflow : IInterfaceDeleteAIReplyRequestWorkflow
    {
        public async Task<object> ExecuteAsync(DeleteAIReplyRequestDto dto)
        {
            return true;
        }
    }
}
