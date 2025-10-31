using System.IO;
using CodeBase.Questionnaires;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    [CreateAssetMenu(fileName = "StandardQuestionnaireJsonParserSo", menuName = "ToolBox/Parsers/Standard Questionnaire Json Parser")]
    public class StandardQuestionnaireJsonParser : BaseTextParserSo
    {
        [SerializeField] private StandardQuestionnaireTemplate standardQuestionnaireTemplate;
        
        private const string QuestionnaireSavePath = "Assets/Questionnaires/Generated/";
        
        public override void Parse(TextAsset textAsset)
        {
            if (textAsset == null)
            {
                Logger.LogError("The text asset is null");
                return;
            }
            
            QuestionnaireWrapper wrapper = JsonConvert.DeserializeObject<QuestionnaireWrapper>(textAsset.text);

            if (wrapper.Questionnaire == null)
            {
                Logger.LogError("The standard questionnaire template is null");
                return;
            }

            standardQuestionnaireTemplate = wrapper.Questionnaire;
            
            BuildQuestionnaireSo();
        }

        private void BuildQuestionnaireSo()
        {
            var asset = CreateInstance<StandardQuestionnaireSo>();
            
            asset.SetData( standardQuestionnaireTemplate ); 
            asset.name = $"{standardQuestionnaireTemplate.ScientificId}_questionnaire";

            var fileName = $"{standardQuestionnaireTemplate.ScientificId}.asset";
            var fullPath = $"{QuestionnaireSavePath}/{fileName}";

            // Ensure the folder exists
            if (!Directory.Exists(QuestionnaireSavePath))
            {
                Directory.CreateDirectory(QuestionnaireSavePath);
                Logger.Log($"Created folder: {QuestionnaireSavePath}");
            }
            
            AssetDatabase.CreateAsset(asset, $"{fullPath}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Logger.Log($"Created StandardQuestionnaireSO asset at {fullPath}");
        }
    }
}
