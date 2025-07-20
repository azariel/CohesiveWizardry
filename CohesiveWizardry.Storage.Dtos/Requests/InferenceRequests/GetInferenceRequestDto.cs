using System.Text.Json.Serialization;
using CohesiveWizardry.Common.Dtos;
using CohesiveWizardry.Storage.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CohesiveWizardry.Storage.Dtos.Requests.InferenceRequests
{
    /// <summary>
    /// Represent a request to get an existing InferenceRequest from the database.
    /// </summary>
    public class GetInferenceRequestDto : IStorageDto, IRequestDto
    {
        [FromRoute]
        [JsonPropertyName("inferenceRequestId")]
        public string InferenceRequestId { get; set; }
    }
}
