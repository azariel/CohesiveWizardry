using System.Text.Json.Serialization;
using CohesiveWizardry.Common.Dtos;
using CohesiveWizardry.Storage.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CohesiveWizardry.Storage.Dtos.Requests.Conversations
{
    /// <summary>
    /// Represent a request to get an existing conversation from the database.
    /// </summary>
    public class GetConversationRequestDto : IStorageDto, IRequestDto
    {
        [FromRoute]
        [JsonPropertyName("conversationId")]
        public string ConversationId { get; set; }
    }
}
