using Cohesive_rp_storage_dtos.Requests.Users;
using Cohesive_rp_storage_dtos.Response.Users;

namespace CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users
{
    public interface IUsersDal
    {
        Task<GetUserResponseDto> GetUserAsync(string userId);
        Task<AddUserResponseDto> AddUserAsync(AddUserRequestDto addUserDto);
        Task<UpdateUserResponseDto> UpdateUserAsync(UpdateUserRequestDto updateUserDto);
        Task<bool> DeleteUserAsync(string userId);
    }
}
