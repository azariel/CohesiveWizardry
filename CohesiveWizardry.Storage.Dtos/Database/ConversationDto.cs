using System.Text.Json.Serialization;
using CohesiveWizardry.Storage.Dtos.Database;
using CohesiveWizardry.Storage.Dtos.Database.GenerationConstraints;

namespace Cohesive_rp_storage_dtos
{
    public class ConversationDto : IGetStorageDto
    {
        [JsonPropertyName("conversationId")]
        public string Id { get; set; }

        [JsonPropertyName("revision")]
        public long Revision { get; set; }

        [JsonPropertyName("lastModifiedAt")]
        public DateTimeOffset LastModifiedAtUtc { get; set; }

        [JsonPropertyName("createdAtUtc")]
        public DateTimeOffset CreatedAtUtc { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("constraintsForAIReplyGen")]
        public List<ConstraintForAIReplyGeneration> ConstraintsForAIReplyGeneration { get; set; }

        // TODO: Add fK to the messages wrapper Dto which is in another tbl
    }
}
