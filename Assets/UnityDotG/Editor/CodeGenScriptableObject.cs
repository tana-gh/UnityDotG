using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace tana_gh.UnityDotG.Editor
{
    public class CodeGenScriptableObject : ScriptableObject
    {
        public List<string> filePaths = new();

        public static readonly string defaultPath = "Assets/UnityDotG/CodeGenScriptableObject.asset";

        internal static void ClearFilePaths()
        {
            Effect(asset => asset.filePaths.Clear());
        }

        internal static void AddFilePath(string path)
        {
            Effect(asset => asset.filePaths.Add(path));
        }

        internal static IEnumerable<string> GetFilePaths()
        {
            return Get(asset => asset.filePaths ?? Enumerable.Empty<string>());
        }

        private static void Effect(Action<CodeGenScriptableObject> effect)
        {
            var asset = FindAsset();
            if (asset == null)
            {
                asset = CreateInstance<CodeGenScriptableObject>();
                Directory.CreateDirectory(Path.GetDirectoryName(defaultPath));
                AssetDatabase.CreateAsset(asset, defaultPath);
            }
            effect(asset);
            EditorUtility.SetDirty(asset);
        }

        private static T Get<T>(Func<CodeGenScriptableObject, T> get)
        {
            var asset = FindAsset();
            if (asset == null)
            {
                return default;
            }
            return get(asset);
        }

        private static CodeGenScriptableObject FindAsset()
        {
            var assetPath =
                AssetDatabase.FindAssets($"t:{nameof(CodeGenScriptableObject)}")
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .FirstOrDefault();
            return assetPath == null ? null : AssetDatabase.LoadAssetAtPath<CodeGenScriptableObject>(assetPath);
        }
    }
}
