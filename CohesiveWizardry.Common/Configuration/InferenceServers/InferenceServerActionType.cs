using System.Text.Json.Serialization;

namespace CohesiveWizardry.Common.Configuration.InferenceServers
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InferenceServerActionType
    {
        MainChat = 0,
        //GenerateSummary = 1,
        //ScanNewNPCs = 2,
        //ScanUpdateNPC = 3,
    }
}
