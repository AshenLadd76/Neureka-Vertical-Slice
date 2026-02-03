using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace FluentUI.Helpers.Editor
{
    /// <summary>
    /// Provides safe loading of USS (Unity StyleSheets) assets from the project.
    /// This loader is designed for Editor use and handles missing files or invalid paths gracefully.
    /// </summary>
    public static class UssStyleSheetLoader
    {
        /// <summary>
        /// Loads a USS file from the specified path in the Unity project.
        /// </summary>
        /// <param name="path">The asset path to the USS file (e.g., "Assets/Editor/USS/TileExtractor.uss").</param>
        /// <param name="elementToApply">
        /// Optional: If specified, the loaded StyleSheet will be applied to this VisualElement.
        /// </param>
        /// <returns>The loaded StyleSheet, or null if the file was not found or failed to load.</returns>
        public static StyleSheet Load(string path, VisualElement elementToApply = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Debug.LogWarning("UssLoader: Provided path is null or empty.");
                return null;
            }

            StyleSheet sheet = null;

            try
            {
                sheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
                if (sheet == null)
                {
                    Debug.LogWarning($"UssLoader: USS file not found at path '{path}'.");
                    return null;
                }
                
                Logger.Log($"UssLoader: Uss file loaded from path '{path}'.");

                elementToApply?.styleSheets.Add(sheet);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"UssLoader: Failed to load USS file at '{path}'. Exception: {ex}");
                return null;
            }

            return sheet;
        }
    }
}



