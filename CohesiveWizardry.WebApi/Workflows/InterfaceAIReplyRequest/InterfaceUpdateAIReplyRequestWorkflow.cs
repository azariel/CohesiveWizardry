using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest
{
    public class InterfaceUpdateAIReplyRequestWorkflow : IInterfaceUpdateAIReplyRequestWorkflow
    {
        public async Task<object> ExecuteAsync(UpdateAIReplyRequestDto dto)
        {
            return true;
        }
    }
}
