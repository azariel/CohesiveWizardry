using System.Text.Json.Serialization;
using CohesiveWizardry.Common.Dtos;
using CohesiveWizardry.Storage.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cohesive_rp_storage_dtos.Requests.Users
{
    /// <summary>
    /// Represent a request to delete an existing user from the database.
    /// </summary>
    public class DeleteUserRequestDto : IStorageDto, IRequestDto
    {
        [FromRoute]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }
}
