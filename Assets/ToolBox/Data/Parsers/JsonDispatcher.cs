using System;
using System.Collections.Generic;
using CodeBase.Questionnaires;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    [CreateAssetMenu(fileName = "JsonDispatcherSo", menuName = "ToolBox/Parsers/JSON Dispatcher")]
    public class JsonDispatcher : BaseDispatcherSo
    {
        private TextAsset _jsonTextAsset;

        [SerializeField] private List<Wormwood.Utils.KeyValuePair<string, BaseTextParserSo>> jsonParserList;

        private Dictionary<string, BaseTextParserSo> _parserDictionary;
        
        private const string MetaData = "MetaData";
       
        
        private void OnEnable()
        {
            InitDictionary();
        }

        private void InitDictionary()
        {
            _parserDictionary = new Dictionary<string, BaseTextParserSo>();

            foreach (var item in jsonParserList)
            {
                if (_parserDictionary.ContainsKey(item.Key))
                {
                    Logger.LogWarning($"Duplicate parser key '{item.Key}' ignored");
                    continue;
                }

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

            _jsonTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            
            var parserType = GetParserType(_jsonTextAsset);
            
            if (string.IsNullOrEmpty(parserType)) return;

            if (_parserDictionary.TryGetValue(parserType, out var parser))
                parser.Parse(_jsonTextAsset, path);
            else
                Logger.LogWarning($"No parser found for type {parserType}");
        }

        /// <summary>
        /// Extracts the parser type from the JSON's MetaData section.
        /// </summary>
        private string GetParserType(TextAsset textAsset)
        {
            if (textAsset == null)
            {
                Logger.LogError("textAsset is null");
                return string.Empty;
            }
            
            try
            {
                QuestionnaireWrapper wrapper = JsonConvert.DeserializeObject<QuestionnaireWrapper>(textAsset.text); 
                
               //var jObject = JsonConvert.DeserializeObject<JObject>(textAsset.text);
                if (wrapper.MetaData == null)
                {
                    Logger.LogWarning("MetaData section missing in JSON");
                    return string.Empty;
                }

                var metaData = wrapper.MetaData;
                
                return metaData.ParseType;
            }
            catch (System.Exception e)
            {
                Logger.LogError($"Failed to parse MetaData: {e}");
                return string.Empty;
            }
        }
    }


    [Serializable]
    public class MetaData
    {
        public string ParseType;

    }
}
