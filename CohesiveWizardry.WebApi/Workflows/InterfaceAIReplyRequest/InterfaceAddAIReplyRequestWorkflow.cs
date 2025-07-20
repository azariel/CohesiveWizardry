using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Configuration;
using CohesiveWizardry.Common.HttpRequest;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest
{
    public class InterfaceAddAIReplyRequestWorkflow : IInterfaceAddAIReplyRequestWorkflow
    {
        public async Task<object> ExecuteAsync(AddAIReplyRequestDto dto)
        {
            var config = CommonConfigurationManager.GetConfigFromMemory();

            // Get request from storage
            (string result, System.Net.HttpStatusCode? resultCode) getCurrentAIReplyRequestResponse = await CustomHttpClient.TryGetAsync(config.StorageSettings.ApiUrl);

            // If request already exists, can't create

            return true;
        }
    }
}
