using System.Text.Json.Serialization;
using CohesiveWizardry.Common.Inference.Models;
using CohesiveWizardry.WebApi.Dtos;

namespace Cohesive_rp_storage_dtos
{
    public class AIReplyRequestDto : IMainApiDto
    {
        [JsonPropertyName("aiReplyRequestId")]
        public string Id { get; set; }

        [JsonPropertyName("createdAtUtc")]
        public DateTimeOffset CreatedAtUtc { get; set; }

        [JsonPropertyName("lastModifiedAt")]
        public DateTimeOffset LastModifiedAtUtc { get; set; }

        [JsonPropertyName("revision")]
        public long Revision { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("actionType")]
        public LLMGenerationRequestActionType ActionType { get; set; }

        // TODO: add a foreign key to ref the selected chat context
    }
}
