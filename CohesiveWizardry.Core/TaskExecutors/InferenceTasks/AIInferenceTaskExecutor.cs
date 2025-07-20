using System.Text;
using Cohesive_rp_storage_dtos;
using CohesiveWizardry.Common.Configuration;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.HttpRequest;
using CohesiveWizardry.Common.Inference.Models;
using CohesiveWizardry.Common.Serialization;
using CohesiveWizardry.Core.Context;
using CohesiveWizardry.Core.Context.Models;
using CohesiveWizardry.Core.Prompt;
using CohesiveWizardry.Core.TaskExecutors.InferenceTasks.Constraints;
using CohesiveWizardry.Core.TaskExecutors.Models;
using CohesiveWizardry.Storage.Dtos.Database.GenerationConstraints;

namespace CohesiveWizardry.Core.TaskExecutors.InferenceTasks
{
    public class AIInferenceTaskExecutor : IInferenceTaskExecutor
    {
        private AIInferenceTask aiInferenceTask;

        public AIInferenceTaskExecutor(AIInferenceTask aiInferenceTask)
        {
            this.aiInferenceTask = aiInferenceTask;
        }

        public async Task Execute()
        {
            // Make sure that the Hard constraints are executed. If they're not, execute them before generating the AI reply
            if (!await ExecuteMissingHardConstraints())
            {
                LoggingManager.LogToFile("3113ef34-7ccf-489f-8be8-084891174f5b", $"Couldn't execute all Hard constraints before generating AI Reply against a free Inference server. The request for generation will be aborted.");
                await AbortTask();
                return;
            }

            // TODO: check if we need to cancel out the Task

            // Build the context required for our prompt
            AIContext aiContext = AIContextBuilder.BuildContext();

            // Build the actual prompt
            string prompt = PromptBuilder.BuildPromptFromAIContext(aiContext);

            // TODO: Call the InferenceManager to query a free and configured LLM for our request (using the aiInferenceTask.actionType)
        }

        private async Task AbortTask()
        {
            var config = CommonConfigurationManager.GetConfigFromMemory();

            // TODO: create a generic retryer function exec
            for (int i = 0; i < 10; i++)
            {
                // refresh inferenceRequest to most current one
                (string result, System.Net.HttpStatusCode? resultCode) getInferenceRequestResponse = await CustomHttpClient.TryGetAsync($"{config.StorageSettings.ApiUrl}/api/InferenceRequests/{aiInferenceTask.InferenceRequestId}");

                if (getInferenceRequestResponse.resultCode != System.Net.HttpStatusCode.OK)
                {
                    await Task.Delay(250);
                    continue;
                }

                AIInferenceTask inferenceTaskToUpdate = null;
                try
                {
                    inferenceTaskToUpdate = JsonCommonSerializer.DeserializeFromString<AIInferenceTask>(getInferenceRequestResponse.result);
                } catch (Exception ex)
                {
                    LoggingManager.LogToFile("45179c18-f733-4ba8-8398-c167b8a9d7ae", $"Couldn't deserialize the result from '{config.StorageSettings.ApiUrl}/api/InferenceRequests/{aiInferenceTask.InferenceRequestId}' into type {nameof(AIInferenceTask)}.");
                    await Task.Delay(250);
                    continue;
                }

                if (inferenceTaskToUpdate == null)
                {
                    await Task.Delay(250);
                    continue;
                }

                // Drop the lock to allow eventual retry
                inferenceTaskToUpdate.Status = LLMGenerationRequestTaskStatus.Pending;

                (string result, System.Net.HttpStatusCode? resultCode) updateInferenceRequestResponse = await CustomHttpClient.TryPostAsync($"{config.StorageSettings.ApiUrl}/api/InferenceRequests/{inferenceTaskToUpdate.InferenceRequestId}",
                    new StringContent(JsonCommonSerializer.SerializeToString(inferenceTaskToUpdate), Encoding.UTF8, "application/json"));

                if (updateInferenceRequestResponse.resultCode == System.Net.HttpStatusCode.OK)
                    break;

                await Task.Delay(250);
            }
        }

        private async Task<bool> ExecuteMissingHardConstraints()
        {
            if (string.IsNullOrWhiteSpace(aiInferenceTask?.ConversationId))
                return true;

            // Get linked conversation from storage
            var config = CommonConfigurationManager.GetConfigFromMemory();
            (string result, System.Net.HttpStatusCode? resultCode) getTiedConversationContextResponse = await CustomHttpClient.TryGetAsync($"{config.StorageSettings.ApiUrl}/api/conversations/{aiInferenceTask.ConversationId}");

            if (getTiedConversationContextResponse.resultCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }

            ConversationDto conversationDto = null;
            try
            {
                conversationDto = JsonCommonSerializer.DeserializeFromString<ConversationDto>(getTiedConversationContextResponse.result);
            } catch (Exception ex)
            {
                LoggingManager.LogToFile("9ce10744-9833-47c9-b06f-621430bb60be", $"Couldn't deserialize the result from '{config.StorageSettings.ApiUrl}/api/conversations/{aiInferenceTask.ConversationId}' into type {nameof(ConversationDto)}.");
                return false;
            }

            if (conversationDto == null)
                return false;

            if (conversationDto.ConstraintsForAIReplyGeneration == null)
                return true;

            foreach (ConstraintForAIReplyGeneration hardConstraint in conversationDto.ConstraintsForAIReplyGeneration.Where(w => w.EnforcementType == ConstraintEnforcementType.Hard))
            {
                // TODO: between each constraints that we need to work on, check if we need to cancel out the Task
                if(!await ConstraintExecutor.Execute(hardConstraint, true))
                    return false;
            }

            return true;
        }
    }
}
