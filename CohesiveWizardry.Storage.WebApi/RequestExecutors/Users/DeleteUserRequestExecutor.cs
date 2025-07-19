using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.RequestExecutors;

namespace CohesiveRp_AI.Storage.RequestExecutors
{
    public class DeleteUserRequestExecutor : IMainRequestExecutor
    {
        private DeleteUserRequestDto deleteUserDto = null;
        private IUsersDal usersDal = null;
        private object response = null;

        public DeleteUserRequestExecutor(
            IUsersDal usersDal,
            DeleteUserRequestDto deleteUserDto)
        {
            this.deleteUserDto = deleteUserDto;
            this.usersDal = usersDal;
        }

        public async Task<bool> ExecuteAsync()
        {
            LoggingManager.LogToFile($"40e088d4-0efa-47fd-a7fe-cac5674a3f30", $"Deleting User with Id [{deleteUserDto?.UserId}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (deleteUserDto?.UserId == null)
            {
                throw new BadRequestWebApiException("b11b13c5-a380-4ac6-8268-39b4eb6c2925", $"Invalid Dto. UserId [{deleteUserDto?.UserId}] was invalid. Request payload was incorrect.");
            }

            // Get User from storage to check if it exists
            var user = await usersDal.TryGetUserAsync(deleteUserDto.UserId);

            if(user == null)
            {
                throw new BadRequestWebApiException("1835ea75-5a77-42ad-b2d0-05e9c00faf77", $"UserId [{deleteUserDto.UserId}] to delete didn't exist in the storage.");
            }

            bool result = await usersDal.TryDeleteUserAsync(deleteUserDto.UserId);
            LoggingManager.LogToFile($"fa2ce79b-19c4-4db3-aaf3-b5002aa9c39d", $"User with Id [{deleteUserDto?.UserId}] was removed.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            response = result;
            return result;
        }

        public async Task<object> GetResponseAsync() => await Task.FromResult(response);
    }
}
