using System.Collections.Generic;
using ToolBox.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    [CreateAssetMenu(fileName = "DispatchManagerSo", menuName = "ToolBox/Parsers/Dispatch Manager", order = 0)]

    public class DispatchManagerSo : ScriptableObject
    {
        [SerializeField] private List<Wormwood.Utils.KeyValuePair<string, BaseDispatcherSo>> dispatchers = new List<Wormwood.Utils.KeyValuePair<string, BaseDispatcherSo>>();

        private Dictionary<string, IDispatcher> _dispatcherDictionary;
        
        private void OnEnable()
        {
            InitDictionary();
        }

        private void InitDictionary()
        {
           // if (!_parserDictionary.IsNullOrEmpty()) return;
            
            _dispatcherDictionary = new Dictionary<string, IDispatcher>();
            
            foreach (var dispatcher in dispatchers)
            {
                
                Logger.Log(dispatcher.Key + " : " + dispatcher.Value);
                
                if (_dispatcherDictionary.ContainsKey(dispatcher.Key)) continue;
                
                if (dispatcher.Value !=  null && dispatcher.Value is IDispatcher iDispatcher)
                    _dispatcherDictionary.Add(dispatcher.Key, iDispatcher);
                else
                    Debug.LogWarning($"dispatcher '{dispatcher.Key}' does not implement IDispatcher!");
            }
        }
        
        public IDispatcher GetDispatcher(string key)
        {
            if (_dispatcherDictionary.IsNullOrEmpty()) return null;
            
            if (_dispatcherDictionary.TryGetValue(key, out var dispatcher))
            {
                Logger.Log( $"Hey we found the correct dispatcher '{key}' its on its way" );
                return dispatcher;
            }

            Debug.LogWarning($"No dispatcher found for key '{key}'");
            return null;
        }
    }
}