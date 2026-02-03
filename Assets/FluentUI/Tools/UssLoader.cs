using UnityEngine;
using UnityEngine.UIElements;

namespace FluentUI.Tools
{
    public static class UssLoader 
    {
        // Load all USS files from a folder inside Resources
        public static void LoadAllUssFromFolder(VisualElement root, string folderPath)
        {
            // Load all StyleSheets in the folder
            StyleSheet[] sheets = Resources.LoadAll<StyleSheet>(folderPath);
            
           
         
            if (sheets == null || sheets.Length == 0)
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

                root.styleSheets.Add(sheet);
            }
        }
    }
}
