using UnityEditor;

namespace tana_gh.UnityDotG.Editor
{
    internal static class MenuSettings
    {
        private const string MenuPathOfGenerateCode = "Tools/UnityDotG/Generate Code";

        [MenuItem(MenuPathOfGenerateCode, priority = 1)]
        private static void GenerateCode()
        {
            CodeGen.GenerateAllFiles();
        }

        private const string MenuPathOfEraseAndGenerateCode = "Tools/UnityDotG/Erase and Generate Code";

        [MenuItem(MenuPathOfEraseAndGenerateCode, priority = 2)]
        private static void EraseAndGenerateCode()
        {
            CodeGen.EraseAllFileContents();
        }

        private const string MenuPathOfGenerateOnCompile = "Tools/UnityDotG/Generate on Compile";

        [MenuItem(MenuPathOfGenerateOnCompile, priority = 3)]
        private static void GenerateOnCompile()
        {
            UnityDotGSettings.GenerateOnCompile = Menu.GetChecked(MenuPathOfGenerateOnCompile);
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
