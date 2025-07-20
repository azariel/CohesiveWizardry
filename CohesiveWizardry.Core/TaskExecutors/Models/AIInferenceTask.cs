using System.Text.Json.Serialization;
using CohesiveWizardry.Common.Inference.Models;

namespace CohesiveWizardry.Core.TaskExecutors.Models
{
    /// <summary>
    /// A task relative to the inference of a new message from the AI, with context handling
    /// </summary>
    public class AIInferenceTask : IInferenceTask
    {
        [JsonPropertyName("inferenceRequestId")]
        public string InferenceRequestId { get; set; }

        [JsonPropertyName("conversationId")]
        public string ConversationId { get; set; }

        [JsonPropertyName("actionType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LLMGenerationRequestActionType ActionType;

        [JsonPropertyName("actionType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LLMGenerationRequestTaskStatus Status;
    }
}
