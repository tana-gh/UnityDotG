using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace tana_gh.UnityDotG.Editor
{
    internal static class CodeGen
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            if (UnityDotGSettings.GenerateRunning)
            {
                UnityDotGSettings.GenerateRunning = false;
            }
            else if (UnityDotGSettings.GenerateOnCompile)
            {
                UnityDotGSettings.GenerateRunning = true;
                try
                {
                    GenerateAllFiles();
                }
                catch
                {
                    UnityDotGSettings.GenerateRunning = false;
                }
            }
        }

        internal static void GenerateAllFiles()
        {
            CodeGenScriptableObject.ClearFilePaths();
            var contexts = GenerateAll();
            WriteAllContexts(contexts);
        }

        internal static void EraseAllFileContents()
        {
            var paths = CodeGenScriptableObject.GetFilePaths();
            if (paths == null) return;
            WriteAllFiles(paths.Select(path => (path, "")), false);
        }

        private static IEnumerable<CodeGenContext> GenerateAll()
        {
            var contexts = new List<CodeGenContext>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(CodeGenAttribute), false).Length > 0)
                    {
                        var context = Generate(type);
                        if (context != null)
                        {
                            contexts.Add(context);
                        }
                    }
                }
            }

            return contexts;
        }

        private static CodeGenContext Generate(Type codeGen)
        {
            var generate = codeGen.GetMethod("Generate");

            if (generate == null || !generate.IsPublic || !generate.IsStatic || generate.ReturnType != typeof(void))
            {
                LogErrorOfNoGenerateMethod(codeGen);
                return null;
            }
            else
            {
                var parameters = generate.GetParameters();

                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(CodeGenContext))
                {
                    LogErrorOfNoGenerateMethod(codeGen);
                    return null;
                }
            }

            var context = new CodeGenContext();
            generate.Invoke(null, new object[] { context });
            return context;
        }

        private static void LogErrorOfNoGenerateMethod(Type codeGen)
        {
            Debug.LogError($"Type {codeGen} does not have a `public static void Generate(CodeGenContext context)` method.");
        }

        private static void WriteAllContexts(IEnumerable<CodeGenContext> contexts)
        {
            WriteAllFiles(contexts.SelectMany(ctx => ctx.PathAndContents), true);
        }

        private static void WriteAllFiles(IEnumerable<(string path, string content)> pathAndContents, bool addToScriptableObject)
        {
            foreach (var (path, content) in pathAndContents)
            {
                WriteFile(path, content, addToScriptableObject);
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private static void WriteFile(string path, string content, bool addToScriptableObject)
        {
            if (addToScriptableObject)
            {
                CodeGenScriptableObject.AddFilePath(path);
            }

            var generatedContent = $"// <auto-generated/>{Environment.NewLine}// This file is generated by UnityDotG. Do not modify it manually.{Environment.NewLine}{content}";
            
            try
            {
                var oldContent = File.ReadAllText(path, Encoding.UTF8);
                if (oldContent == generatedContent) return;
            }
            catch (IOException)
            {
                // File does not exist, NOP
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, generatedContent, Encoding.UTF8);
        }
    }
}
