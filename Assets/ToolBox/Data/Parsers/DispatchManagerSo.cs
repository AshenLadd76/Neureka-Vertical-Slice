using System.Collections.Generic;
using System.IO;
using ToolBox.Extensions;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    /// <summary>
    /// Manages dispatching imported files to the appropriate parser based on file extension.
    /// </summary>
    
    [CreateAssetMenu(fileName = "DispatchManagerSo", menuName = "ToolBox/Parsers/Dispatch Manager", order = 0)]
    public class DispatchManagerSo : ScriptableObject, IFileDispatcher
    {
        [SerializeField] private List<Wormwood.Utils.KeyValuePair<string, BaseDispatcherSo>> dispatchers = new ();

        private Dictionary<string, BaseDispatcherSo> _dispatcherDictionary;
        
        private void OnEnable() => InitDictionary();
        

        /// <summary>
        ///Initializes the dispatcher dictionary from the serialized list.
        /// Logs warnings for duplicates or null values.
        /// </summary>

        private void InitDictionary()
        {
            _dispatcherDictionary = new Dictionary<string, BaseDispatcherSo>();
            
            foreach (var dispatcher in dispatchers)
            {
                if (_dispatcherDictionary.ContainsKey(dispatcher.Key))
                {
                    Logger.LogWarning($"Duplicate dispatcher key '{dispatcher.Key}' ignored");
                    continue;
                }

                if (dispatcher.Value !=null)
                    _dispatcherDictionary.Add(dispatcher.Key, dispatcher.Value);
                else
                    Logger.LogWarning($"dispatcher '{dispatcher.Key}' does not implement IDispatcher!");
            }
        }

        
      
        /// <summary>
        /// Handles an imported file by dispatching it to the appropriate parser based on its extension.
        /// </summary>
        /// <param name="assetPath">The path to the imported asset file.</param>
        /// <param name="fullPath">Absolute system path of the file.</param>
        
        public void Dispatch(string assetPath, string fullPath)
        {
            if( _dispatcherDictionary.IsNullOrEmpty() ) InitDictionary();
            
            var ext = GetFileExtension(assetPath);
            
            Logger.Log( $"Dispatch asset path '{assetPath}' ext: {ext}" );
            
            if (string.IsNullOrEmpty(ext)) return;

            var dispatcher = GetDispatcher(ext);

            if (dispatcher == null)
            {
                Logger.Log($"File does not contain dispatcher for file type: {ext}");
                return;
            }
            
            dispatcher.Dispatch(assetPath);
        }

        
        /// <summary>
        /// Extracts the file extension from a given path.
        /// </summary>
        /// <param name="path">The path to extract the extension from.</param>
        /// <returns>The file extension in lowercase, without the leading dot, or an empty string if invalid.</returns>
        
        private string GetFileExtension(string path) => string.IsNullOrEmpty(path) ? string.Empty : Path.GetExtension(path)?.TrimStart('.').ToLower();
        
        
        /// <summary>
        /// Returns the dispatcher associated with a given key (file extension).
        /// </summary>
        /// <param name="key">The file extension key.</param>
        /// <returns>The associated <see cref="BaseDispatcherSo"/> or null if none found.</returns>

        private BaseDispatcherSo GetDispatcher(string key)
        {
            if (_dispatcherDictionary.IsNullOrEmpty()) return null;
            
            if (_dispatcherDictionary.TryGetValue(key, out var dispatcher))
            {
                Logger.Log( $"Hey we found the correct dispatcher '{key}' its on its way" );
                return dispatcher;
            }

            Logger.LogWarning($"No dispatcher found for key '{key}'");
            return null;
        }
    }
}