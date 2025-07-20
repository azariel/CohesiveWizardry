using CohesiveWizardry.Common.Exceptions;
using CohesiveWizardry.Core.TaskExecutors.Models;

namespace CohesiveWizardry.Core.TaskExecutors.InferenceTasks
{
    public static class InferenceTaskExecutoryFactory
    {
        public static IInferenceTaskExecutor GenerateTaskExecutor(IInferenceTask inferenceTask)
        {
            if(inferenceTask == null)
                return null;

            switch (inferenceTask)
            {
                case AIInferenceTask aiInferenceTask:
                    return new AIInferenceTaskExecutor(aiInferenceTask);
                default:
                    throw new CommonException("8337eb47-1430-4b46-a0ef-f13420617efe", $"Inference Task of type [{inferenceTask}] is unhandled.");
            }
        }
    }
}
