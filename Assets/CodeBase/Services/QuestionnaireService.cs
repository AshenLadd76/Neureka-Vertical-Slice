using System;
using System.Collections.Generic;
using CodeBase.Questionnaires;
using CodeBase.UiComponents.Pages;
using ToolBox.Data.Parsers;
using ToolBox.Extensions;
using ToolBox.Helpers;
using ToolBox.Messaging;
using ToolBox.Services;
using ToolBox.Utils.Validation;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Services
{
    /// <summary>
    /// Service responsible for handling requests to display questionnaire.
    /// Listens for request messages and produces Questionnaire UI pages
    /// based on the requested StandardQuestionnaireSo assets.
    /// </summary>
    
    public class QuestionnaireService : BaseService
    {
        // Reference to the UIDocument containing the root UI element.
        private UiDocumentManager _uiDocumentManager;
        
        private VisualElement _safeAreaContainer;
        
        //Dictionary of scriptable objects that contain questionniare data
        private readonly Dictionary<string, StandardQuestionnaireSo> _standardQuestionnaires = new();
        
        private IReadOnlyDictionary<string, StandardQuestionnaireSo> StandardQuestionnaires => _standardQuestionnaires;
        
        private ISerializer _jsonSerializer;
        
        private const string PathToQuestionnaires = "Questionnaires";
        public const string OnRequestQuestionnaireMessage = "OnRequestQuestionnaire";
        public const string OnRequestAssessmentQuestionnaireMessage = "OnRequestAssessmentQuestionnaire";
        public const string OnRequestAllQuestionnaireDataMessage= "OnRequestAllQuestionnaireData";
        
        private void Awake() => Init();
        

        private void Init()
        {
            _uiDocumentManager = GetComponent<UiDocumentManager>();
            
            ObjectValidator.Validate(_uiDocumentManager);

            _safeAreaContainer = _uiDocumentManager.SafeAreaContainer;
            
            _jsonSerializer = new JsonSerializer();
            
            LoadQuestionnairesIntoDictionary();
        }
        

        /// <summary>
        /// Loads all StandardQuestionnaireSo assets from Resources
        /// and stores them in the dictionary with normalized IDs.
        /// </summary>
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
                var normalizedId = NormalizeId(rawId);

                if (!_standardQuestionnaires.TryAdd(normalizedId, standardQuestionnaireSo))
                    Logger.LogWarning($"Duplicate questionnaire ID: {rawId}");
            }
        }
        
        /// <summary>
        /// Retrieves the StandardQuestionnaireTemplate by ID.
        /// Logs an error if the ID is empty or not found.
        /// </summary>
        /// <param name="id">The questionnaire ID.</param>
        /// <returns>The corresponding StandardQuestionnaireTemplate, or null if not found.</returns>
        private StandardQuestionnaireTemplate GetQuestionnaireData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Logger.LogError("Questionnaire ID is empty");
                return null;
            }

            var normalizedId = NormalizeId(id);
            if (_standardQuestionnaires.TryGetValue(normalizedId, out var so))
                return so.Data;

            Logger.LogError($"Questionnaire not found: {id}");
            return null;
        }
        
        /// <summary>
        /// Builds and displays the questionnaire page.
        /// </summary>
        /// <param name="id">The questionnaire ID.</param>
        /// <param name="parentDocument">The parent document for an assessment, only relevant if questionnaire is called from an assessment</param> 
        /// <param name="onComplete">Callback to invoke when the assessment is complete.</param>
        private void BuildQuestionnairePage(string id, IDocument parentDocument = null,Action onComplete = null)
        {
            var data = GetQuestionnaireData(id);
            
            var builder = new QuestionnairePageBuilder(data, _safeAreaContainer, _jsonSerializer, parentDocument, onComplete);
            
            builder.Build();
        }
        

        /// <summary>
        /// Handles requests for a standard questionnaire.
        /// Builds and displays the questionnaire page.
        /// </summary>
        /// <param name="id">The questionnaire ID.</param>
        private void OnRequestQuestionnaire(string id) => BuildQuestionnairePage(id);
        
        /// <summary>
        /// Handles requests for an assessment questionnaire with a completion callback.
        /// Builds and displays the questionnaire page.
        /// </summary>
        /// <param name="id">The questionnaire ID.</param>
        /// <param name="parentDocument">The parent document for an assessment, only relevant if questionnaire is called from an assessment</param> 
        /// <param name="onComplete">Callback to invoke when the assessment is complete.</param>
        private void OnRequestAssessmentQuestionnaire(string id,IDocument parentDocument, Action onComplete) => BuildQuestionnairePage(id,parentDocument,onComplete);


        /// <summary>
        /// Returns the dictionary of questionnaire data to requester
        /// </summary>
        /// <param name="callback">Callback that is invoked when a request is recieved. Passes the QuestionnairDictionary</param>
        private void OnRequestAllQuestionnaireData(Action<IReadOnlyDictionary<string, StandardQuestionnaireSo> > callback)
        {
            callback?.Invoke(StandardQuestionnaires);
        }
        
        // Subscribes to message bus events when the service is enabled.
        protected override void SubscribeToService()
        {
            MessageBus.AddListener<string>(OnRequestQuestionnaireMessage,OnRequestQuestionnaire );
            MessageBus.AddListener<string, IDocument,  Action>( OnRequestAssessmentQuestionnaireMessage, OnRequestAssessmentQuestionnaire );
            MessageBus.AddListener<Action<IReadOnlyDictionary<string, StandardQuestionnaireSo>>>( OnRequestAllQuestionnaireDataMessage, OnRequestAllQuestionnaireData );
        }

        // Unsubscribes from message bus events when the service is disabled.
        protected override void UnsubscribeFromService()
        {
            MessageBus.RemoveListener<string>(OnRequestQuestionnaireMessage,OnRequestQuestionnaire );
            MessageBus.RemoveListener<string, IDocument, Action>( OnRequestAssessmentQuestionnaireMessage, OnRequestAssessmentQuestionnaire );
        }
        
        //Little method to normalise the questionnaire id 
        private string NormalizeId(string rawId) => rawId.Trim().ToLowerInvariant();
    }
}
