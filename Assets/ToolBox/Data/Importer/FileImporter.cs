#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Importer
{
    public class FileImporter : AssetPostprocessor
    {
        public static event Action<string> OnFileImported;

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var assetPath in importedAssets)
                ProcessFile(assetPath);
        }

        private static void ProcessFile(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath)) return;

            var fullPath = GetFullPath(assetPath);
            
            Logger.Log($"Processing file {fullPath}");
            
            if (!File.Exists(fullPath))
            {
                Logger.Log($"File does not exist: {fullPath}");
                return;
            }
            
            OnFileImported?.Invoke(assetPath);
        }
        
         private static string GetFullPath(string path) => Path.Combine(Application.dataPath, path.Substring("Assets/".Length));
    }
}
#endif

