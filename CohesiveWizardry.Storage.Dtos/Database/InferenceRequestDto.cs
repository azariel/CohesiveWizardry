using System.Text.Json.Serialization;
using CohesiveWizardry.Common.Inference.Models;
using CohesiveWizardry.Storage.Dtos.Database;

namespace Cohesive_rp_storage_dtos
{
    public class InferenceRequestDto : IGetStorageDto
    {
        [JsonPropertyName("inferenceRequestId")]
        public string Id { get; set; }

        [JsonPropertyName("revision")]
        public long Revision { get; set; }

        [JsonPropertyName("lastModifiedAt")]
        public DateTimeOffset LastModifiedAtUtc { get; set; }

        [JsonPropertyName("createdAtUtc")]
        public DateTimeOffset CreatedAtUtc { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LLMGenerationRequestTaskStatus Status { get; set; }

        // TODO: add a field that foreign key a selected Character
    }
}
