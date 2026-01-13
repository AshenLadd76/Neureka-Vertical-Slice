#if UNITY_EDITOR
using System.IO;
using ToolBox.Data.Parsers;
using UnityEditor;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Importer
{
    /// <summary>
    /// Handles automatic processing of files imported into the Unity project.
    /// <para>
    /// Scans imported assets in the specified folder and dispatches them to the 
    /// first <see cref="IFileDispatcher"/> ScriptableObject found ( by rights there should only be one. ).
    /// </para>
    /// <para>
    /// Caches the dispatcher to avoid repeated searches on subsequent imports.
    /// </para>
    /// </summary>
    public class FileImporter : AssetPostprocessor
    {
        private static readonly IFileDispatcher DispatchManager;

        private const string SearchPath = "Assets/Resources/FileImports";
        
        //Bit cheeky but searching for the So filename saves time and effort
        private const string SearchForObject = "t:DispatchManagerSo"; 
        
        private const string SubPath = "Assets/";
        
        private const string TargetFolder = "Assets/Resources/Questionnaires";


        static FileImporter()
        {
            var guids = AssetDatabase.FindAssets(SearchForObject, new[] { SearchPath });

            if (guids.Length == 0)
            {
                Logger.LogWarning("No DispatchManagerSo found in project.");
                return;
            }
            
            
            var so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guids[0]));

            if (so is IFileDispatcher dispatcher)
            {
                DispatchManager = dispatcher;
                Logger.Log("DispatchManager initialized successfully.");
            }
            else
            {
                Logger.LogWarning("Found ScriptableObject does not implement IFileDispatcher.");
            }
        }
         
     
        /// <summary>
        /// Called automatically by Unity when assets are imported, deleted, or moved.
        /// Dispatches imported files to the appropriate <see cref="IFileDispatcher"/>.
        /// </summary>
        /// <param name="importedAssets">Paths of newly imported assets.</param>
        /// <param name="deletedAssets">Paths of deleted assets.</param>
        /// <param name="movedAssets">New paths of moved assets.</param>
        /// <param name="movedFromAssetPaths">Old paths of moved assets.</param>

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var assetPath in importedAssets)
            {
                if (!assetPath.StartsWith(TargetFolder)) continue;
                
                ProcessFile(assetPath);
            }
        }

        /// <summary>
        /// Processes a single file: checks existence and dispatches it.
        /// </summary>
        /// <param name="assetPath">Path to the asset, relative to the Unity project.</param>

        private static void ProcessFile(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                Logger.Log( $"Asset path is null or empty" );
                return;
            }

            var fullPath = GetFullPath(assetPath);
            
            if (!File.Exists(fullPath))
            {
                Logger.Log($"File does not exist: {fullPath}");
                return;
            }
            
            HandleDispatch(assetPath,fullPath);
        }
        
        /// <summary>
        /// Finds a valid <see cref="IFileDispatcher"/> and sends the file for processing.
        /// </summary>
        /// <param name="assetPath">Unity asset path of the file.</param>
        /// <param name="fullPath">Absolute system path of the file.</param>
        
        private static void HandleDispatch(string assetPath, string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath) || string.IsNullOrEmpty(assetPath)) return;

            DispatchManager?.Dispatch(assetPath, fullPath);
        }
        
        /// <summary>
        /// Converts a Unity asset path to an absolute system path.
        /// </summary>
        /// <param name="path">Unity asset path starting with "Assets/".</param>
        /// <returns>Absolute system path to the file.</returns>
         private static string GetFullPath(string path) => Path.Combine(Application.dataPath, path.Substring(SubPath.Length));
    }
}
#endif

