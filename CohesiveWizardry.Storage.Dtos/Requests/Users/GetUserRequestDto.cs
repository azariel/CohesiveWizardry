using System.Text.Json.Serialization;
using CohesiveWizardry.Common.Dtos;
using CohesiveWizardry.Storage.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CohesiveWizardry.Storage.Dtos.Requests.Users
{
    /// <summary>
    /// Represent a request to get an existing user from the database.
    /// </summary>
    public class GetUserRequestDto : IStorageDto, IRequestDto
    {
        [FromRoute]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }
}
