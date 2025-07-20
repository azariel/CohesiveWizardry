using System.Text;
using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Configuration;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Common.HttpRequest;
using CohesiveWizardry.Common.Serialization;
using CohesiveWizardry.Core.TaskExecutors;
using CohesiveWizardry.Core.TaskExecutors.Models;
using CohesiveWizardry.Storage.Dtos.Requests.InferenceRequests;
using CohesiveWizardry.Storage.Dtos.Responses.InferenceRequests;
using CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest.Abstractions;

namespace CohesiveWizardry.WebApi.Workflows.InterfaceAIReplyRequest
{
    public class InterfaceAddAIReplyRequestWorkflow : IInterfaceAddAIReplyRequestWorkflow
    {
        public async Task<object> ExecuteAsync(AddAIReplyRequestDto dto)
        {
            LoggingManager.LogToFile($"8c6b10a1-a100-4dcf-93c5-abed49cad03d", $"Adding new Inference Request [{dto?.Id}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            // Validate
            if (string.IsNullOrWhiteSpace(dto?.Id))
                throw new BadRequestWebApiException("c46c605e-93a4-4e95-a018-b1237a94d29c", $"Invalid Dto. Id is invalid. Request payload was incorrect.");

            if (string.IsNullOrWhiteSpace(dto?.ConversationId))
                throw new BadRequestWebApiException("bc9a8602-d18b-4337-9c0d-520208c8679e", $"Invalid Dto. ConversationId is invalid. Request payload was incorrect.");

            var config = CommonConfigurationManager.GetConfigFromMemory();

            // Validate that the conversation exist in storage before anything
            (string result, System.Net.HttpStatusCode? resultCode) getCurrentConversationResponse = await CustomHttpClient.TryGetAsync($"{config.StorageSettings.ApiUrl}/api/conversations/{dto.ConversationId}");

            if (getCurrentConversationResponse.resultCode != System.Net.HttpStatusCode.OK || string.IsNullOrWhiteSpace(getCurrentConversationResponse.result))
            {
                throw new WebApiException("5ed57db5-c415-43f2-9ac4-35dd33716b32", $"The call to Storage for Conversation with Id [{dto.ConversationId}] returned status code [{getCurrentConversationResponse.resultCode}]. The query to add a new AI Reply Request will be aborted.");
            }

            // Get inference request from storage
            (string result, System.Net.HttpStatusCode? resultCode) getCurrentAIReplyRequestResponse = await CustomHttpClient.TryGetAsync($"{config.StorageSettings.ApiUrl}/api/InferenceRequests/{dto.Id}");

            switch (getCurrentAIReplyRequestResponse.resultCode)
            {
                case System.Net.HttpStatusCode.OK:
                    // The inference request already exist
                    AddInferenceRequestResponseDto currentInferenceRequestInDb = null;
                    try
                    {
                        currentInferenceRequestInDb = JsonCommonSerializer.DeserializeFromString<AddInferenceRequestResponseDto>(getCurrentAIReplyRequestResponse.result);
                    } catch (Exception e)
                    {
                        throw new WebApiException("ec0d8493-e67b-47e5-bdb9-7f7acefd8c99", $"The inferenceRequest get from the Storage webapi couldn't be deserialized.");
                    }

                    throw new ConflictWebApiException("764e3122-d4f4-45bd-8035-8e615f5aa32d", $"AI Reply Request underlying InferenceRequest [{dto.Id}] already exists and is of status [{currentInferenceRequestInDb.Status}].");
                case System.Net.HttpStatusCode.NotFound:
                    // The inference request doesn't exist, Add the new inference Request
                    await ExecuteGenerationAIReply(dto);
                    break;
                default:
                    // Unknown response
                    throw new WebApiException("37f8eeb2-d1a0-4ea1-97f4-ab972901145f", $"The inference request behind the AI Reply Request [{dto.Id}] was queried to Storage webApi, but returned Status [{getCurrentAIReplyRequestResponse.resultCode}]. The AI Reply Request creation will be aborted.");
            }

            LoggingManager.LogToFile($"cf9ce761-93a7-41a5-aca3-eafc50a9c71f", $"New Inference Request with Id [{dto.Id}] was added.", logVerbosity: LoggingManager.LogVerbosity.Verbose);
            return true;
        }

        private async Task ExecuteGenerationAIReply(AddAIReplyRequestDto dto)
        {
            if (dto == null)
                return;

            if (dto.ActionType != Common.Inference.Models.LLMGenerationRequestActionType.MainChatAIReplyGenerationAction)
            {
                // As long as we're not queuing an AI reply request that isn't tied to the Main Chat, we can afford to queue it and process it in the backend
                dto.Status = Common.Inference.Models.LLMGenerationRequestTaskStatus.Pending;
                await QueueInferenceRequestInStorageWebApi(dto);
                return;
            }

            // The request for AI reply is tied to the Main Chat, we must prioritize it, so we'll create an ongoing inference request in Storage webApi and launch a FireAndForget async task right away to process the inferenceRequest
            dto.Status = Common.Inference.Models.LLMGenerationRequestTaskStatus.InProgress;
            await QueueInferenceRequestInStorageWebApi(dto);

            // Execute the Task
            _ = TaskExecutor.ExecuteTask(new AIInferenceTask
            {
                InferenceRequestId = dto.Id,
                ConversationId = dto.ConversationId,
                ActionType = dto.ActionType
            });
        }

        private async Task<AddInferenceRequestResponseDto> QueueInferenceRequestInStorageWebApi(AddAIReplyRequestDto dto)
        {
            if (dto == null)
                return null;

            var requestDto = new AddInferenceRequestDto
            {
                Id = dto.Id,
            };

            var config = CommonConfigurationManager.GetConfigFromMemory();
            (string result, System.Net.HttpStatusCode? resultCode) createAIReplyRequestResponse = await CustomHttpClient.TryPostAsync($"{config.StorageSettings.ApiUrl}/api/InferenceRequests",
                new StringContent(JsonCommonSerializer.SerializeToString(requestDto), Encoding.UTF8, "application/json"));

            if (createAIReplyRequestResponse.resultCode != System.Net.HttpStatusCode.OK)
            {
                throw new WebApiException("daffdb6e-bd7e-4188-9b69-d07f30a40bb1", $"Tried to create a new inference request to Storage WebApi, but the service returned status [{createAIReplyRequestResponse.resultCode}]. The AI Reply Request creation will be aborted.");
            }

            // Deserialize model
            try
            {
                var response = JsonCommonSerializer.DeserializeFromString<AddInferenceRequestResponseDto>(createAIReplyRequestResponse.result);
                return response;
            } catch (Exception e)
            {
                throw new WebApiException("d2494aed-5965-4e65-99c7-a540e2bf4f0e", $"Tried to create a new inference request to Storage WebApi. The service was successful, but the response couldn't be deserialized.");
            }
        }
    }
}
