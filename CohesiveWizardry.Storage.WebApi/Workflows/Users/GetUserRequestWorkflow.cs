using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.Dtos.Requests.Users;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class GetUserRequestWorkflow : IGetUserRequestWorkflow
    {
        private IUsersDal usersDal = null;

        public GetUserRequestWorkflow(
            IUsersDal usersDal)
        {
            this.usersDal = usersDal;
        }

        public async Task<object> ExecuteAsync(GetUserRequestDto getUserDto)
        {
            LoggingManager.LogToFile($"a1d1800f-fac0-4840-b796-bcb166ee8d9d", $"Getting User with Id [{getUserDto?.UserId}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (getUserDto?.UserId == null)
            {
                throw new BadRequestWebApiException("87590262-78f5-47c4-ba7c-520cb64ce5ab", $"Invalid Dto. UserId [{getUserDto?.UserId}] was invalid. Request payload was incorrect.");
            }

            // Get User from storage to check if it already exists
            var response = await usersDal.GetUserAsync(getUserDto.UserId);
            LoggingManager.LogToFile($"46864055-5c9e-480f-a11c-bfd53a6fcc67", $"User with Id [{getUserDto?.UserId}] was Get.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            return response;
        }
    }
}
