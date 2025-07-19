using CohesiveWizardry.Common.Serialization;

namespace CohesiveWizardry.Common.Configuration
{
    /// <summary>
    /// Manager around the Common configuration.
    /// </summary>
    public static class CommonConfigurationManager
    {
        // ********************************************************************
        //                            Constants
        // ********************************************************************
        private const string CONFIG_FILE_NAME = "CohesiveWizardryCommon-config.json";

        // ********************************************************************
        //                            Private
        // ********************************************************************
        private static CommonConfiguration CohesiveRpConfig;

        // ********************************************************************
        //                            Public
        // ********************************************************************
        public static CommonConfiguration GetConfigFromMemory()
        {
            return CohesiveRpConfig ?? ReloadConfig();
        }

        public static CommonConfiguration ReloadConfig()
        {
            // Create default config if doesn't exists
            if (!File.Exists(CONFIG_FILE_NAME))
                SaveConfigInMemoryToDisk();

            string _ConfigFileContent = File.ReadAllText(CONFIG_FILE_NAME);
            CohesiveRpConfig = JsonCommonSerializer.DeserializeFromString<CommonConfiguration>(_ConfigFileContent);
            return CohesiveRpConfig;
        }

        public static void SetConfigInMemory(CommonConfiguration aCohesiveRpConfig)
        {
            CohesiveRpConfig = aCohesiveRpConfig;
        }

        public static void SaveConfigInMemoryToDisk()
        {
            if (CohesiveRpConfig == null)
                CohesiveRpConfig = new CommonConfiguration();

            string _SerializedConfig = JsonCommonSerializer.SerializeToString(CohesiveRpConfig);
            File.WriteAllText(CONFIG_FILE_NAME, _SerializedConfig);
        }
    }
}
