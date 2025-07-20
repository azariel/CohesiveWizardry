using System.Text.Json.Serialization;
using CohesiveWizardry.Common.Dtos;
using CohesiveWizardry.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Cohesive_rp_storage_dtos.Requests.Users
{
    /// <summary>
    /// Represent a request to delete an existing AI Reply Request from the tasks to process.
    /// </summary>
    public class DeleteAIReplyRequestDto : IMainApiDto, IRequestDto
    {
        [FromRoute]
        [JsonPropertyName("aiReplyRequestId")]
        public string AIReplyRequestId { get; set; }
    }
}
