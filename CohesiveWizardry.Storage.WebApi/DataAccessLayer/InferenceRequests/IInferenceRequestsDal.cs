using CohesiveWizardry.Storage.Dtos.Requests.InferenceRequests;
using CohesiveWizardry.Storage.Dtos.Responses.InferenceRequests;

namespace CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users
{
    public interface IInferenceRequestsDal
    {
        Task<GetInferenceRequestResponseDto> GetInferenceRequestAsync(string inferenceRequestId);
        Task<AddInferenceRequestResponseDto> AddInferenceRequestAsync(AddInferenceRequestDto addInferenceRequestDto);
        Task<UpdateInferenceRequestResponseDto> UpdateInferenceRequestAsync(UpdateInferenceRequestDto updateInferenceRequestDto);
        Task<bool> DeleteInferenceRequestAsync(string inferenceRequestId);
    }
}
