using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Diagnostics;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.RequestExecutors;

namespace CohesiveRp_AI.Storage.RequestExecutors
{
    public class GetUserRequestExecutor : IMainRequestExecutor
    {
        private GetUserRequestDto getUserDto = null;
        private IUsersDal usersDal = null;
        private object response = null;

        public GetUserRequestExecutor(
            IUsersDal usersDal,
            GetUserRequestDto getUserDto)
        {
            this.getUserDto = getUserDto;
            this.usersDal = usersDal;
        }

        public async Task<bool> ExecuteAsync()
        {
            LoggingManager.LogToFile($"a1d1800f-fac0-4840-b796-bcb166ee8d9d", $"Getting User with Id [{getUserDto?.UserId}].", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            if (getUserDto?.UserId == null)
            {
                throw new BadRequestWebApiException("87590262-78f5-47c4-ba7c-520cb64ce5ab", $"Invalid Dto. UserId [{getUserDto?.UserId}] was invalid. Request payload was incorrect.");
            }

            // Get User from storage to check if it already exists
            response = await usersDal.TryGetUserAsync(getUserDto.UserId);
            LoggingManager.LogToFile($"46864055-5c9e-480f-a11c-bfd53a6fcc67", $"User with Id [{getUserDto?.UserId}] was Get.", logVerbosity: LoggingManager.LogVerbosity.Verbose);

            return true;
        }

        public async Task<object> GetResponseAsync() => await Task.FromResult(response);
    }
}
