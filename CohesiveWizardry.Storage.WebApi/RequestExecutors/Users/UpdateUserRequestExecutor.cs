using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.RequestExecutors;

namespace CohesiveRp_AI.Storage.RequestExecutors
{
    public class UpdateUserRequestExecutor : IMainRequestExecutor
    {
        private UpdateUserRequestDto updateUserDto = null;
        private IUsersDal usersDal = null;
        private object response = null;

        public UpdateUserRequestExecutor(
            IUsersDal usersDal,
            UpdateUserRequestDto updateUserDto)
        {
            this.updateUserDto = updateUserDto;
            this.usersDal = usersDal;
        }

        public async Task<bool> ExecuteAsync()
        {
            LoggingManager.LogToFile($"76758e9e-10b7-4e6b-adc6-421aeddc71af", $"Updating User with Id [{updateUserDto?.Id}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (updateUserDto?.Id == null)
            {
                throw new BadRequestWebApiException("31de46b2-4f1d-4c30-9215-430e7aadaa38", $"Invalid Dto. UserId [{updateUserDto?.Id}] was invalid. Request payload was incorrect.");
            }

            // Get User from storage to check if it already exists
            var user = await usersDal.TryGetUserAsync(updateUserDto.Id);

            if (user == null)
            {
                response = $"Can't update user. User with id [{updateUserDto.Id}] doesn't exists.";
                return false;
            }

            // Update the new user
            var userResult = await usersDal.TryUpdateUserAsync(updateUserDto);
            LoggingManager.LogToFile($"47bf5139-042d-4fe4-a27d-e44db97c6377", $"User with Id [{updateUserDto?.Id}] was updated.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            response = userResult;
            return true;
        }

        public async Task<object> GetResponseAsync() => await Task.FromResult(response);
    }
}
