using UnityEngine;
using UnityEngine.UIElements;

namespace FluentUI.Tools
{
    public static class UssLoader 
    {
        /// <summary>
        /// Loads all USS files from a Resources folder and adds them to the root VisualElement.
        /// </summary>
        /// <param name="root">Root VisualElement to apply stylesheets to.</param>
        /// <param name="folderPath">Path inside Resources folder.</param>

        public static void LoadAllUssFromFolder(VisualElement root, string folderPath)
        {
            // Load all StyleSheets in the folder
            StyleSheet[] sheets = Resources.LoadAll<StyleSheet>(folderPath);
            
         
            if (sheets.Length == 0)
            {
                Debug.LogWarning($"No USS found in Resources/{folderPath}");
                return;
            }

            foreach (var sheet in sheets)
            {
                if (sheet == null)
                {
                    Debug.LogWarning($"Found invalid StyleSheet in Resources/{folderPath}");
                    continue;
                }

                if (!root.styleSheets.Contains(sheet))
                    root.styleSheets.Add(sheet);
            }
        }
    }
}
