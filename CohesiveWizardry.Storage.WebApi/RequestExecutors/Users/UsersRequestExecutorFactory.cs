using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveRp_AI.Storage.RequestExecutors;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.Dtos.Requests;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;

namespace CohesiveWizardry.Storage.WebApi.RequestExecutors.Users
{
    public static class UsersRequestExecutorFactory
    {
        public static IMainRequestExecutor GenerateExecutor(IUsersDal usersDal, IStorageDto requestDto)
        {
            switch (requestDto)
            {
                case AddUserRequestDto addUserDto:
                    return new AddUserRequestExecutor(usersDal, addUserDto);
                case UpdateUserRequestDto updateUserDto:
                    return new UpdateUserRequestExecutor(usersDal, updateUserDto);
                case GetUserRequestDto getUserDto:
                    return new GetUserRequestExecutor(usersDal, getUserDto);
                case DeleteUserRequestDto deleteUserDto:
                    return new DeleteUserRequestExecutor(usersDal, deleteUserDto);
                default:
                    throw new BadRequestWebApiException("4d0ed33d-aa14-4acb-8a5d-55174e0fea75", $"The Dto model [{requestDto.GetType()}] type is unhandled.");
            }
        }
    }
}
