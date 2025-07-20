using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class AddUserRequestWorkflow : IAddUserRequestWorkflow
    {
        private IUsersDal usersDal = null;

        public AddUserRequestWorkflow(
            IUsersDal usersDal)
        {
            this.usersDal = usersDal;
        }

        public async Task<object> ExecuteAsync(AddUserRequestDto addUserDto)
        {
            LoggingManager.LogToFile($"78253ec3-4055-4dfe-8c7b-df9d961835fa", $"Adding new User [{addUserDto?.Username}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (string.IsNullOrWhiteSpace(addUserDto?.Id))
                throw new BadRequestWebApiException("c3e052ce-388f-4840-b011-744acc0c1eb7", $"Invalid Dto. Id is invalid. Request payload was incorrect.");

            if (string.IsNullOrWhiteSpace(addUserDto?.Username))
                throw new BadRequestWebApiException("0af6b6b3-a3ff-40e5-a56f-f8a9ca952cb1", $"Invalid Dto. Username [{addUserDto?.Username}] was invalid. Request payload was incorrect.");

            // Get User from storage to check if it already exists
            var user = await usersDal.GetUserAsync(addUserDto.Id);

            if(user != null)
                throw new ConflictWebApiException("e1dc8d69-8bad-4d03-8cad-e4d010ba1a5d", $"User with Id [{addUserDto.Id}] to add already exist in the storage.");

            // Add the new user
            var userResult = await usersDal.AddUserAsync(addUserDto);

            LoggingManager.LogToFile($"47dd4eb7-0729-4e53-a8d5-b924c4b1a2d8", $"New User [{addUserDto?.Username}] with Id [{addUserDto?.Id}] was added.", logVerbosity: LoggingManager.LogVerbosity.Verbose);
            return userResult;
        }
    }
}
