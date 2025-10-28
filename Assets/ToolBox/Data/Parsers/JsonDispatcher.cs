using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    [CreateAssetMenu(fileName = "JsonDispatcherSo", menuName = "ToolBox/Parsers/JSON Dispatcher")]
    public class JsonDispatcher : BaseDispatcherSo
    {
        private TextAsset _jsonTextAsset;

        [SerializeField] private List<Wormwood.Utils.KeyValuePair<string, BaseTextParserSo>> jsonParserList ;
        
        private Dictionary<string, IParser<TextAsset>> _parserDictionary;


        private void OnEnable()
        {
            InitDictionary();
        }

        private void InitDictionary()
        {
            _parserDictionary = new Dictionary<string, IParser<TextAsset>>();

            foreach (var item in jsonParserList  )
            {
                if( _parserDictionary.ContainsKey(item.Key) ) continue;
                
                _parserDictionary.Add(item.Key, item.Value);
            }
        }


        public override void Dispatch(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Logger.LogError("path is null or empty");
                return;
            }
            
            _jsonTextAsset  = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            
            var jsonObj = JObject.Parse(_jsonTextAsset.text);

            if (jsonObj["MetaData"] == null)
            {
                return;
            }

            // peek at the parser type
            string parserType = (string)jsonObj["MetaData"]["ParserType"];
            
            Logger.Log( $"Parser Type : {parserType}" );

            if (string.IsNullOrEmpty(parserType)) return;
            
            if(_parserDictionary.TryGetValue(parserType, out var parser))
                parser.Parse(_jsonTextAsset);
            else
                Logger.LogWarning($"No parser found for type {parserType}");

        }
    }

    public class MetaData
    {
        public string ParserType;

    }
    
}
