using System.Collections.Generic;
using CodeBase.Questionnaires;
using CodeBase.UiComponents.Pages;
using ToolBox.Data.Parsers;
using ToolBox.Extensions;
using ToolBox.Messenger;
using ToolBox.Utils.Validation;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Factories
{
    public class QuestionnaireFactory : MonoBehaviour
    {
        private UIDocument _uiDocument;
        
        private VisualElement _rootVisualElement;
        
        private const string PathToQuestionnaires = "Questionnaires";
        
        //Dictionary to store all questionniares scriptable objects

        private readonly Dictionary<string, StandardQuestionnaireSo> _standardQuestionnaires = new();
        
        
        public const string OnRequestQuestionnaireMessage = "OnRequestQuestionnaire";


        private void OnEnable()
        {
            MessageBus.Instance.AddListener<string>(OnRequestQuestionnaireMessage,OnRequestQuestionnaire );
        }

        private void OnDisable()
        {
            MessageBus.Instance.RemoveListener<string>(OnRequestQuestionnaireMessage,OnRequestQuestionnaire );
        }

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            ObjectValidator.Validate(_uiDocument);
            
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            LoadQuestionnairesIntoDictionary();
        }

        private void LoadQuestionnairesIntoDictionary()
        {
            var allQuestionnaires = Resources.LoadAll<StandardQuestionnaireSo>(PathToQuestionnaires);

            if (allQuestionnaires.IsNullOrEmpty())
            {
                Logger.LogError("No Questionnaires found in Resources, You probably need to import them");
                return;
            }

            foreach (var q in allQuestionnaires)
            {
                if (q.Data == null)
                {
                    Logger.LogError($"Questionnaire {q.name} has no data");
                    continue;
                }

                if (string.IsNullOrEmpty(q.Data.QuestionnaireID))
                {
                    Logger.LogError($"No questionnaire id found in Questionnaires data { q.name }");
                    continue;
                }
                
                if (!_standardQuestionnaires.TryAdd(q.Data.QuestionnaireID, q))
                    Logger.LogWarning($"Duplicate questionnaire ID: {q.Data.QuestionnaireID}");
                else
                    Logger.Log( $"Added questionnaire ID: {q.Data.QuestionnaireID}" );
                
            }
        }
        
        private StandardQuestionnaireTemplate GetQuestionnaire(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Logger.LogError("Questionnaire ID is empty");
                return null;
            }
            
            
            if (_standardQuestionnaires.TryGetValue(id.ToUpper().Trim(), out var so))
            {
                if (so.Data == null)
                {
                    Logger.LogError($"Questionnaire data is missing for ID: {id}");
                    return null;
                }
                
                return so.Data;
            }

            Logger.LogError($"Questionnaire not found: {id}");
            return null;
        }

        private void OnRequestQuestionnaire(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Logger.LogError("Questionnaire ID is empty");
                return;
            }
            
            var questionnaireData = GetQuestionnaire(id);
            
            if (questionnaireData == null) return;

            new QuestionnairePageBuilder(questionnaireData, _rootVisualElement);
        }
    }
}
