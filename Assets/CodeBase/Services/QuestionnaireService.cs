using System;
using System.Collections.Generic;
using CodeBase.Questionnaires;
using CodeBase.UiComponents.Pages;
using ToolBox.Data.Parsers;
using ToolBox.Extensions;
using ToolBox.Helpers;
using ToolBox.Messenger;
using ToolBox.Utils.Validation;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Services
{
    public class QuestionnaireService : MonoBehaviour
    {
        private UIDocument _uiDocument;
        
        private VisualElement _rootVisualElement;
        
        private const string PathToQuestionnaires = "Questionnaires";
        
        //Dictionary to store all questionniares scriptable objects
        private readonly Dictionary<string, StandardQuestionnaireSo> _standardQuestionnaires = new();
        
        public const string OnRequestQuestionnaireMessage = "OnRequestQuestionnaire";
        public const string OnRequestAssessmentQuestionnaireMessage = "OnRequestAssessmentQuestionnaire";
        
        private ISerializer _jsonSerializer;
        
        private void OnEnable()
        {
            MessageBus.Instance.AddListener<string>(OnRequestQuestionnaireMessage,OnRequestQuestionnaire );
            MessageBus.Instance.AddListener<string, Action>( OnRequestAssessmentQuestionnaireMessage, OnRequestAssessmentQuestionnaire );
        }

       
        private void OnDisable()
        {
            MessageBus.Instance.RemoveListener<string>(OnRequestQuestionnaireMessage,OnRequestQuestionnaire );
            MessageBus.Instance.RemoveListener<string, Action>( OnRequestAssessmentQuestionnaireMessage, OnRequestAssessmentQuestionnaire );
        }

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            ObjectValidator.Validate(_uiDocument);
            
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            _jsonSerializer = new JsonSerializer();
            
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

            foreach (var standardQuestionnaireSo in allQuestionnaires)
            {
                if (standardQuestionnaireSo.Data == null)
                {
                    Logger.LogError($"Questionnaire {standardQuestionnaireSo.name} has no data");
                    continue;
                }

                var rawId = standardQuestionnaireSo.Data.QuestionnaireID;

                if (string.IsNullOrEmpty(rawId))
                {
                    Logger.LogError($"No questionnaire ID found in {standardQuestionnaireSo.name}");
                    continue;
                }

                // Normalize the ID once here
                var normalizedId = rawId.Trim().ToLowerInvariant();

                if (!_standardQuestionnaires.TryAdd(normalizedId, standardQuestionnaireSo))
                    Logger.LogWarning($"Duplicate questionnaire ID: {rawId}");
                else
                    Logger.Log($"Added questionnaire ID: {rawId}");
            }
        }
        
        private StandardQuestionnaireTemplate GetQuestionnaire(string id)
        {
            
            if (_standardQuestionnaires.TryGetValue(id.ToLower().Trim(), out var so))
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

        private StandardQuestionnaireTemplate GetQuestionnaireData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Logger.LogError("Questionnaire ID is empty");
                return null;
            }

            var normalizedId = id.Trim().ToLowerInvariant();
            if (_standardQuestionnaires.TryGetValue(normalizedId, out var so))
                return so.Data;

            Logger.LogError($"Questionnaire not found: {id}");
            return null;
        }

        private void OnRequestQuestionnaire(string id)
        {
            var questionnaireData = GetQuestionnaireData(id);

            if (questionnaireData == null)
            {
                Logger.LogError($"Questionnaire not found: {id}");
                return;
            }

            var questionnairePageBuilder = new QuestionnairePageBuilder(questionnaireData, _rootVisualElement, _jsonSerializer);
            questionnairePageBuilder.Build();
            
        }
        
        private void OnRequestAssessmentQuestionnaire(string id, Action action)
        {
            var questionnaireData = GetQuestionnaireData(id);

            if (questionnaireData == null)
            {
                Logger.LogError($"Questionnaire not found: {id}");
                return;
            }
            
            var questionnairePageBuilder = new QuestionnairePageBuilder(questionnaireData, _rootVisualElement, _jsonSerializer, action);
            questionnairePageBuilder.Build();
        }

    }
}
