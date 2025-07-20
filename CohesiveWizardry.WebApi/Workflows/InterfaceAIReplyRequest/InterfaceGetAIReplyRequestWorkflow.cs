using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest
{
    public class InterfaceGetAIReplyRequestWorkflow : IInterfaceGetAIReplyRequestWorkflow
    {
        public async Task<object> ExecuteAsync(GetAIReplyRequestDto dto)
        {
            return true;
        }
    }
}
