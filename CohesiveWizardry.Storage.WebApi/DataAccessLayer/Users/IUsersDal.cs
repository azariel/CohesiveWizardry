using Cohesive_rp_storage_dtos.Requests.Users;
using Cohesive_rp_storage_dtos.Response.Users;

namespace CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users
{
    public interface IUsersDal
    {
        Task<GetUserResponseDto> TryGetUserAsync(string userId);
        Task<AddUserResponseDto> TryAddUserAsync(AddUserRequestDto addUserDto);
        Task<UpdateUserResponseDto> TryUpdateUserAsync(UpdateUserRequestDto updateUserDto);
        Task<bool> TryDeleteUserAsync(string userId);
    }
}
