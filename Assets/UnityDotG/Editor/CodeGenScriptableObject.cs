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
        private readonly List<string> tmpFilePaths = new();

        public static string DefaultPath { get; } = "Assets/UnityDotGSettings/CodeGenScriptableObject.asset";

        internal static void AddFilePath(string path)
        {
            Effect(asset => asset.tmpFilePaths.Add(path));
        }

        internal static void Apply()
        {
            Effect(asset =>
            {
                asset.tmpFilePaths.Sort();
                if (!asset.filePaths.SequenceEqual(asset.tmpFilePaths))
                {
                    asset.filePaths.Clear();
                    asset.filePaths.AddRange(asset.tmpFilePaths);
                    EditorUtility.SetDirty(asset);
                }
                asset.tmpFilePaths.Clear();
            });
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
                asset = CreateAsset();
            }
            effect(asset);
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

        private static CodeGenScriptableObject CreateAsset()
        {
            var asset = CreateInstance<CodeGenScriptableObject>();
            Directory.CreateDirectory(Path.GetDirectoryName(DefaultPath));
            AssetDatabase.CreateAsset(asset, DefaultPath);
            AssetDatabase.ImportAsset(DefaultPath);
            return asset;
        }
    }
}
