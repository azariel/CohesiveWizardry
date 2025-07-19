using CohesiveWizardry.Storage.Dtos.Requests;
using CohesiveWizardry.Storage.WebApi.DataAccessLayer.Users;
using CohesiveWizardry.Storage.WebApi.RequestExecutors.Users;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public class UsersWorkflow : IUsersWorkflow
    {
        private IUsersDal cohesiveRpDal = null;

        public UsersWorkflow(IUsersDal CohesiveRpDal)
        {
            cohesiveRpDal = CohesiveRpDal;
        }

        public async Task<object> ExecuteAsync(IStorageDto postDto)
        {
            if (postDto == null)
                return null;

            var executor = UsersRequestExecutorFactory.GenerateExecutor(cohesiveRpDal, postDto);

            if (executor == null)
                return null;// TODO: throw?

            await executor.ExecuteAsync();
            return await executor.GetResponseAsync();
        }
    }
}
