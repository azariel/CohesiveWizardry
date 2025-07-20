using CohesiveWizardry.Common.Exceptions;
using CohesiveWizardry.Core.TaskExecutors.InferenceTasks;
using CohesiveWizardry.Core.TaskExecutors.Models;

namespace CohesiveWizardry.Core.TaskExecutors
{
    public static class TaskExecutor
    {
        /// <summary>
        /// Handle the execution of any task relative to CohesiveWizardry.
        /// </summary>
        public static async Task ExecuteTask(ICohesiveWizardryTask task)
        {
            switch (task)
            {
                case IInferenceTask inferenceTask:
                    await InferenceTaskExecutoryFactory.GenerateTaskExecutor(inferenceTask)?.Execute();
                    break;
                default:
                    throw new CommonException("244817ab-9835-45aa-91a8-7f0a6be89adc", $"Task [{task?.GetType()}] is unhandled.");
            }
        }
    }
}
