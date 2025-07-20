using Cohesive_rp_storage_dtos.Requests.Users;
using Cohesive_rp_storage_dtos.Response.Users;

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
