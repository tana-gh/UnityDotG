using UnityEditor;

namespace tana_gh.UnityDotG.Editor
{
    public static class UnityDotGSettings
    {
        internal const string configNameOfGenerateOnCompile = "tana_gh.UnityDotG.GenerateOnCompile";

        public static bool GenerateOnCompile
        {
            get => GetBoolConfigValue(configNameOfGenerateOnCompile, true);
            set => SetBoolConfigValue(configNameOfGenerateOnCompile, value);
        }

        internal const string configNameOfGenerateRunning = "tana_gh.UnityDotG.GenerateRunning";

        public static bool GenerateRunning
        {
            get => GetBoolConfigValue(configNameOfGenerateRunning, false);
            set => SetBoolConfigValue(configNameOfGenerateRunning, value);
        }

        private static bool GetBoolConfigValue(string configName, bool defaultValue)
        {
            if (bool.TryParse(EditorUserSettings.GetConfigValue(configName), out var value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        private static void SetBoolConfigValue(string configName, bool value)
        {
            EditorUserSettings.SetConfigValue(configName, value.ToString());
        }
    }
}
