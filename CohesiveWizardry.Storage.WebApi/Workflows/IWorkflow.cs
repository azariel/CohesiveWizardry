using CohesiveWizardry.Storage.Dtos.Requests;

namespace CohesiveWizardry.Storage.WebApi.Workflows
{
    public interface IWorkflow
    {
        Task<object> ExecuteAsync(IStorageDto dto);
    }
}
