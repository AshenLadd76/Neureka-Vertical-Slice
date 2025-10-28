using System.Collections.Generic;
using ToolBox.Extensions;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    [CreateAssetMenu(fileName = "DispatchManagerSo", menuName = "ToolBox/Parsers/Dispatch Manager", order = 0)]

    public class DispatchManagerSo : ScriptableObject
    {
        [SerializeField] private List<Wormwood.Utils.KeyValuePair<string, BaseDispatcherSo>> dispatchers = new List<Wormwood.Utils.KeyValuePair<string, BaseDispatcherSo>>();

        private Dictionary<string, BaseDispatcherSo> _dispatcherDictionary;
        
        private void OnEnable()
        {
            InitDictionary();
        }

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
        
        public BaseDispatcherSo GetDispatcher(string key)
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