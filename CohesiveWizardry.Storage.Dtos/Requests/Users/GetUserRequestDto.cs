using System.Text.Json.Serialization;
using CohesiveWizardry.Common.Dtos;
using CohesiveWizardry.Storage.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cohesive_rp_storage_dtos.Requests.Users
{
    /// <summary>
    /// Represent a request to add a new user to the database.
    /// </summary>
    public class GetUserRequestDto : IStorageDto, IRequestDto
    {
        [FromRoute]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }
}
