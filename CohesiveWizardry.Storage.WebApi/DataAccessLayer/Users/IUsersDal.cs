using CohesiveWizardry.Storage.Dtos.Requests.Users;
using CohesiveWizardry.Storage.Dtos.Responses.Conversations;
using CohesiveWizardry.Storage.Dtos.Responses.Users;

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
