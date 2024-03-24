using UnityEditor;

namespace tana_gh.UnityDotG.Editor
{
    internal static class MenuSettings
    {
        static MenuSettings()
        {
            Menu.SetChecked(MenuPathOfGenerateOnCompile, UnityDotGSettings.GenerateOnCompile);
        }

        private const string MenuPathOfGenerateCode = "Tools/UnityDotG/Generate Code";

        [MenuItem(MenuPathOfGenerateCode)]
        private static void GenerateCode()
        {
            CodeGen.GenerateAllFiles();
        }

        private const string MenuPathOfGenerateOnCompile = "Tools/UnityDotG/Generate on Compile";

        [MenuItem(MenuPathOfGenerateOnCompile)]
        private static void GenerateOnCompile()
        {
            Menu.SetChecked(MenuPathOfGenerateOnCompile, UnityDotGSettings.GenerateOnCompile);
            UnityDotGSettings.GenerateOnCompile = Menu.GetChecked(MenuPathOfGenerateOnCompile);
        }
    }
}
