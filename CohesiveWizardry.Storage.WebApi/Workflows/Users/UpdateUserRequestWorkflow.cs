using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.Dtos.Requests.Users;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class UpdateUserRequestWorkflow : IUpdateUserRequestWorkflow
    {
        private IUsersDal usersDal = null;

        public UpdateUserRequestWorkflow(
            IUsersDal usersDal)
        {
            this.usersDal = usersDal;
        }

        public async Task<object> ExecuteAsync(UpdateUserRequestDto updateUserDto)
        {
            LoggingManager.LogToFile($"76758e9e-10b7-4e6b-adc6-421aeddc71af", $"Updating User with Id [{updateUserDto?.Id}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (updateUserDto?.Id == null)
            {
                throw new BadRequestWebApiException("31de46b2-4f1d-4c30-9215-430e7aadaa38", $"Invalid Dto. UserId [{updateUserDto?.Id}] was invalid. Request payload was incorrect.");
            }

            // Get User from storage to check if it already exists
            var user = await usersDal.GetUserAsync(updateUserDto.Id);

            if (user == null)
                throw new ConflictWebApiException("6820777c-6fd8-4495-b8fb-e25a32422148", $"Can't update User with Id [{updateUserDto.Id}]. User does not exist in storage.");

            // Update the new user
            var userResult = await usersDal.UpdateUserAsync(updateUserDto);
            LoggingManager.LogToFile($"47bf5139-042d-4fe4-a27d-e44db97c6377", $"User with Id [{updateUserDto?.Id}] was updated.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            return userResult;
        }
    }
}
