#if UNITY_EDITOR
using System;
using System.IO;
using CodeBase.Questionnaires;
using ToolBox.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using JsonSerializer = ToolBox.Helpers.JsonSerializer;
using Logger = ToolBox.Utils.Logger;


namespace ToolBox.Data.Parsers
{
    /// <summary>
    /// Parses a standard questionnaire JSON file and creates a corresponding 
    /// <see cref="StandardQuestionnaireSo"/> asset in the project.
    /// </summary>
    
    [CreateAssetMenu(fileName = "StandardQuestionnaireJsonParserSo", menuName = "ToolBox/Parsers/Standard Questionnaire Json Parser")]
    public class StandardQuestionnaireJsonParser : BaseTextParserSo
    {
        [SerializeField] private StandardQuestionnaireTemplate standardQuestionnaireTemplate;
        [SerializeField] private string questionnaireSavePath = "Assets/Resources/Questionnaires/";
        [SerializeField] private string questionnaireSuffix = "_questionnaire";
        [SerializeField] private string textAssetExt = ".asset";
        
        [SerializeField] private Sprite iconImage;
        
        private ISerializer _serializer;

        private void OnEnable()
        {
            _serializer = new JsonSerializer();
        }
        
        /// <summary>
        /// Parses a JSON TextAsset representing a standard questionnaire and creates a corresponding SO asset.
        /// </summary>
        /// <param name="textAsset">The JSON TextAsset to parse.</param>
        /// <param name="pathToSourceFile">The file path of the source JSON file, used for cleanup after parsing.</param>
        
        public override void Parse(TextAsset textAsset, string pathToSourceFile)
        {
            if (textAsset == null)
            {
                Logger.LogError("The text asset is null");
                return;
            }

            if (string.IsNullOrEmpty(pathToSourceFile))
            {
                Logger.LogError("The path to the source file is null or empty");
                return;
            }

            QuestionnaireWrapper wrapper = _serializer.Deserialize<QuestionnaireWrapper>(textAsset.text); 

            if (wrapper.Questionnaire == null)
            {
                Logger.LogError("The standard questionnaire template is null");
                return;
            }

            standardQuestionnaireTemplate = wrapper.Questionnaire;
            
            BuildQuestionnaireSo(pathToSourceFile);

            
        }


        private const string iconPath = "Sprites/Questionnaires/";
        private Sprite LoadIconImage(string iconName)
        {
            if (string.IsNullOrEmpty(iconName))
            {
                Logger.LogError("The icon name is null or empty");
                return null;
            }
            
            Logger.Log(iconPath + iconName);
            
            var sprite = Resources.Load<Sprite>(iconPath + iconName);

            if (sprite == null)
            {
                Logger.LogError("The icon resource could not be found: " + iconPath + iconName);
                return null;
            }
            
            return sprite;
        }
        
        /// <summary>
        /// Creates a <see cref="StandardQuestionnaireSo"/> asset from the parsed template 
        /// and saves it to the configured folder. Deletes the source JSON file afterward.
        /// </summary>
        /// <param name="pathToSourceFile">The full path to the source JSON file.</param>
        
        private void BuildQuestionnaireSo(string pathToSourceFile)
        {
            var asset = CreateInstance<StandardQuestionnaireSo>();
            
            asset.SetData( standardQuestionnaireTemplate );

            var icon = LoadIconImage(standardQuestionnaireTemplate.QuestionnaireIcon);

            if (!icon)
            {
                Logger.LogError( $"Icon could not be loaded from {pathToSourceFile}" );
            }

            asset.SetIcon(icon);
            
            asset.name = $"{standardQuestionnaireTemplate.ScientificId}{questionnaireSuffix}";

            var fileName = $"{standardQuestionnaireTemplate.ScientificId}{textAssetExt}";
            var fullPath = Path.Combine( questionnaireSavePath, fileName );
            
            if (!Directory.Exists(questionnaireSavePath))
            {
                Directory.CreateDirectory(questionnaireSavePath);
                Logger.Log($"Created folder: {questionnaireSavePath}");
            }
//             
// #if UNITY_EDITOR       
//             EditorUtility.SetDirty(asset);
// #endif
            
            try
            {
                AssetDatabase.CreateAsset(asset, $"{fullPath}");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to create SO asset at {fullPath}: {e.Message}");
                return;
            }
            
            Logger.Log($"Created StandardQuestionnaireSO asset at {fullPath}");
            
            if (!DeleteSourceFile(pathToSourceFile))
                Logger.LogWarning($"Failed to delete source file: {pathToSourceFile}");
        }
    }
}
#endif
