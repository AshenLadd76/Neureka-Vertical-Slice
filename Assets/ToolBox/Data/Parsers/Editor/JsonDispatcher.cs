using System;
using System.Collections.Generic;
using CodeBase.Questionnaires;
using ToolBox.Extensions;
using ToolBox.Helpers;
using UnityEditor;
using UnityEngine;
using JsonSerializer = ToolBox.Helpers.JsonSerializer;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers.Editor
{
    /// <summary>
    /// Dispatches JSON files to the appropriate parser based on the parser type 
    /// specified in the JSON's MetaData section. 
    /// </summary>
    
    
    [CreateAssetMenu(fileName = "JsonDispatcherSo", menuName = "ToolBox/Parsers/JSON Dispatcher")]
    public class JsonDispatcher : BaseDispatcherSo
    {
        [SerializeField] private List<Wormwood.Utils.KeyValuePair<string, BaseTextParserSo>> jsonParserList = new();

        private Dictionary<string, BaseTextParserSo> _parserDictionary;
        
        private const string MetaData = "MetaData";
        
        private ISerializer _serializer;


        private void OnEnable()
        {
            _serializer = new JsonSerializer();
            
            InitDictionary();
        }




        private void InitDictionary()
        {
            _parserDictionary = new Dictionary<string, BaseTextParserSo>();

            if (jsonParserList.IsNullOrEmpty())
            {
                jsonParserList = new List<Wormwood.Utils.KeyValuePair<string, BaseTextParserSo>>();
            }

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


        /// <summary>
        /// Dispatches a JSON file to the appropriate parser based on the parser type
        /// defined in the file's MetaData.
        /// </summary>
        /// <param name="path">The full path to the JSON file asset.</param>

        public override void Dispatch(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Logger.LogError("path is null or empty");
                return;
            }

            var jsonTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            if (jsonTextAsset == null)
            {
                Logger.LogError("Failed to load json text");
                return;
            }
            
            var parserType = GetParserType(jsonTextAsset);

            if (string.IsNullOrEmpty(parserType))
            {  
                Logger.LogWarning("Failed to get parser type");
                return;
            }

            if (_parserDictionary.TryGetValue(parserType, out var parser))
                parser.Parse(jsonTextAsset, path);
            else
                Logger.LogWarning($"No parser found for type {parserType}");
        }

        /// <summary>
        /// Extracts the parser type from the JSON's MetaData section.
        /// </summary>
        /// <param name="textAsset">The JSON TextAsset to extract the parser type from.</param>
        /// <returns>The parser type string if found; otherwise, an empty string.</returns>
        private string GetParserType(TextAsset textAsset)
        {
            if (textAsset == null)
            {
                Logger.LogError("textAsset is null");
                return string.Empty;
            }
            
            try
            {
                QuestionnaireWrapper wrapper = _serializer.Deserialize<QuestionnaireWrapper>(textAsset.text); 
                
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


    
    /// <summary>
    /// Represents the MetaData section of a JSON questionnaire file.
    /// </summary>
    [Serializable]
    public class MetaData
    {
        /// <summary>
        /// Type of parser to use for this JSON file.
        /// </summary>
        public string ParseType;

    }
}
