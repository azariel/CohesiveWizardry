using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.RequestExecutors;

namespace CohesiveRp_AI.Storage.RequestExecutors
{
    public class AddUserRequestExecutor : IMainRequestExecutor
    {
        private AddUserRequestDto addUserDto = null;
        private IUsersDal usersDal = null;
        private object response = null;

        public AddUserRequestExecutor(
            IUsersDal usersDal,
            AddUserRequestDto AddUserDto)
        {
            this.addUserDto = AddUserDto;
            this.usersDal = usersDal;
        }

        public async Task<bool> ExecuteAsync()
        {
            LoggingManager.LogToFile($"78253ec3-4055-4dfe-8c7b-df9d961835fa", $"Adding new User [{addUserDto?.Username}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (string.IsNullOrWhiteSpace(addUserDto?.Username))
            {
                throw new BadRequestWebApiException("0af6b6b3-a3ff-40e5-a56f-f8a9ca952cb1", $"Invalid Dto. Username [{addUserDto?.Username}] was invalid. Request payload was incorrect.");
            }

            // TODO: Get User from storage to check if it already exists
            var user = await usersDal.TryGetUserAsync(addUserDto.Id);

            if(user != null)
            {
                throw new BadRequestWebApiException("e1dc8d69-8bad-4d03-8cad-e4d010ba1a5d", $"User with Username [{addUserDto.Username}] to add already exist in the storage.");
            }

            // Add the new user
            var userResult = await usersDal.TryAddUserAsync(addUserDto);

            LoggingManager.LogToFile($"47dd4eb7-0729-4e53-a8d5-b924c4b1a2d8", $"New User [{addUserDto?.Username}] was added.", logVerbosity: LoggingManager.LogVerbosity.Verbose);
            response = userResult;
            return true;
        }

        public async Task<object> GetResponseAsync() => await Task.FromResult(response);
    }
}
