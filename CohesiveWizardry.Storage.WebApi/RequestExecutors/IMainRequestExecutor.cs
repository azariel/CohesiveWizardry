namespace CohesiveWizardry.Storage.WebApi.RequestExecutors
{
    public interface IMainRequestExecutor
    {
        Task<bool> ExecuteAsync();
        Task<object> GetResponseAsync();
    }
}
