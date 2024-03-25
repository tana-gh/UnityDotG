using UnityEditor;

namespace tana_gh.UnityDotG.Editor
{
    internal static class MenuSettings
    {
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
            UnityDotGSettings.GenerateOnCompile = !Menu.GetChecked(MenuPathOfGenerateOnCompile);
            Menu.SetChecked(MenuPathOfGenerateOnCompile, UnityDotGSettings.GenerateOnCompile);
        }

        [MenuItem(MenuPathOfGenerateOnCompile, validate = true)]
        private static bool ValidateGenerateOnCompile()
        {
            Menu.SetChecked(MenuPathOfGenerateOnCompile, UnityDotGSettings.GenerateOnCompile);
            return true;
        }
    }
}
