using System.Text.Json.Serialization;
using CohesiveWizardry.Storage.Dtos.Database;

namespace Cohesive_rp_storage_dtos
{
    public class UserDto : IGetStorageDto
    {
        [JsonPropertyName("userId")]
        public string Id { get; set; }

        [JsonPropertyName("revision")]
        public long Revision { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("lastModifiedAt")]
        public DateTimeOffset LastModifiedAtUtc { get; set; }

        [JsonPropertyName("createdAtUtc")]
        public DateTimeOffset CreatedAtUtc { get; set; }

        // TODO: add a field that foreign key a selected Character
    }
}
