#if UNITY_EDITOR
using System;
using System.IO;
using ToolBox.Data.Parsers;
using UnityEditor;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Importer
{
    public class FileImporter : AssetPostprocessor
    {
        private static string _fullPath;

        private const string DispatcherManagerPath =  "Assets/Resources/FileImports/DispatchManagerSo.asset";

        private static DispatchManagerSo _dispatcherManagerSo;

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            _dispatcherManagerSo = AssetDatabase.LoadAssetAtPath<DispatchManagerSo>(DispatcherManagerPath);

            if (_dispatcherManagerSo == null)
            {
                Logger.LogError("Failed to load parser manager so");
                return;
            }

            foreach (var assetPath in importedAssets)
                ProcessFile(assetPath);
        }

        private static void ProcessFile(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath)) return;

            var fullPath = GetFullPath(assetPath);
            if (!File.Exists(fullPath))
            {
                Logger.Log($"File does not exist: {fullPath}");
                return;
            }

            var ext = GetFileType(assetPath);
            if (string.IsNullOrEmpty(ext)) return;

            var dispatcher = _dispatcherManagerSo.GetDispatcher(ext);
            dispatcher?.Dispatch(assetPath);
        }


        private static string GetFileType(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Logger.Log("Path not found");
                return string.Empty;
            }

            var ext = Path.GetExtension(path)?.TrimStart('.').ToLower();
            if (string.IsNullOrEmpty(ext)) return string.Empty;
            
            Logger.Log($"Importing file: {path}, type: {ext}");
            return ext;
        }


        private static string GetFullPath(string path) => Path.Combine(Application.dataPath, path.Substring("Assets/".Length));
    }
}
#endif

