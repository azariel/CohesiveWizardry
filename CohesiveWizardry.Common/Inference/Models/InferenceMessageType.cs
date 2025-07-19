namespace CohesiveWizardry.Common.Inference.Models
{
    public enum InferenceMessageType
    {
        direct, // direct injection to the prompt
        system,
        user,
        assistant
    }
}
