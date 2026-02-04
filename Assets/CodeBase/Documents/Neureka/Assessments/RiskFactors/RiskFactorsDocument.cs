using System;
using System.Collections.Generic;
using CodeBase.Services;
using ToolBox.Extensions;
using ToolBox.Messaging;
using ToolBox.Services.Data;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Assessments.RiskFactors
{
    /// <summary>
    /// Controls the Risk Factors assessment document lifecycle.
    /// 
    /// Responsible for:
    /// - Loading assessment configuration data
    /// - Building and registering document pages
    /// - Managing assessment flow (new, continuing, completed)
    /// - Requesting questionnaires and handling completion callbacks
    /// - Cleaning up persisted assessment data and returning to the hub
    /// </summary>
    
    public class RiskFactorsDocument : BaseDocument
    {
        private readonly RiskFactorsDataHandler _riskFactorsDataHandler;
        
        private const string RiskFactorsSoPath = "Assessments/RiskFactors/RiskFactorsSO";
        
        private RiskFactorsSO _riskFactorsSo;
        
        /// <summary>
        /// Creates a new RiskFactorsDocument instance.
        /// </summary>
        /// <param name="fileDataService">
        /// Service used to persist and retrieve assessment progress data.
        /// Must not be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="fileDataService"/> is null.
        /// </exception>
        
        public RiskFactorsDocument(IFileDataService fileDataService)
        {
            if( fileDataService == null )
                throw new ArgumentNullException(nameof(fileDataService), "IFileDataService cannot be null.");
            
            _riskFactorsDataHandler = new RiskFactorsDataHandler(fileDataService);
        }

        /// <summary>
        /// Builds the Risk Factors assessment.
        /// 
        /// Responsibilities:
        /// - Building the assessment schedule
        /// - Building the intro, continue, and outro pages
        /// </summary>
        /// <param name="root">The parent visual element. Must not be null.</param>
       
        public override void Build(VisualElement root)
        {
            base.Build(root);

            if (!LoadRiskFactorsSo())
            {
                Logger.LogError("RiskFactorsSO could not be loaded!");
                return;
            }

            AddPageRecipes();
            
            Open( root );
        }
        
        
        /// <summary>
        /// Opens and manages the state of the active assessment.
        /// 
        /// Responsibilities:
        /// - Checking the current status of the assessment
        /// - Opening the corresponding pages
        /// </summary>
        /// <param name="root">The parent visual element. Must not be null.</param>
        
        public override void Open(VisualElement root)
        {
            //Open a welcome back/ continue page when previous progress exists.
            if (_riskFactorsDataHandler.CheckAssessmentState() == AssessmentState.Continuing)
                LoadContinuePage();
            else
                CheckAssessmentProgress();
        }
        
        /// <summary>
        /// Adds the intro, continue, and outro pages to the assessment.
        /// </summary>
        
        private void AddPageRecipes()
        {
            ActivePages.Clear();
            
            CreatePage(PageID.RiskFactorsIntro, _riskFactorsSo.IntroTitle, _riskFactorsSo.IntroButtonText, _riskFactorsSo.IntroBlurbContents, OnIntroCompleted);
            CreatePage(PageID.RiskFactorsContinue, _riskFactorsSo.ContinueTitle, _riskFactorsSo.ContinueButtonText, _riskFactorsSo.ContinueBlurbContents, CheckAssessmentProgress);
            CreatePage(PageID.RiskFactorsOutro, _riskFactorsSo.OutroTitle, _riskFactorsSo.OutroButtonText, _riskFactorsSo.OutroBlurbContents, OnAssessmentCompleted);
        }

        
        /// <summary>
        /// Loads ScriptableObject data from the Resources folder.
        /// </summary>
        /// <returns>True if loaded successfully; otherwise, false.</returns>
        
        private bool LoadRiskFactorsSo()
        {
            _riskFactorsSo = Resources.Load<RiskFactorsSO>(RiskFactorsSoPath);
            
            if (_riskFactorsSo == null)
            {
                Logger.LogError("Failed to load RiskFactorsSO from Resources!");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Creates a page with the specified content and registers it in ActivePages.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <param name="title">The page title.</param>
        /// <param name="buttonText">The button text for the page.</param>
        /// <param name="blurbs">The list of blurb content.</param>
        /// <param name="onCompleted">Optional callback when the page is completed.</param>

        private void CreatePage(PageID pageId, string title, string buttonText, List<BlurbContent> blurbs, Action onCompleted = null)
        {
            if (blurbs.IsNullOrEmpty())
            {
                Logger.LogError($"RiskFactorsSO list for {pageId} is empty or null!");
                return;
            }

            var content = new InfoPageContent(title, buttonText, blurbs, onCompleted);
            
           ActivePages[pageId] = new InfoPage(this, content, DocumentRoot);
        }
        
        
        /// <summary>
        /// Determines the current assessment state and routes the user
        /// to the appropriate page or questionnaire flow.
        /// </summary>

        private void CheckAssessmentProgress()
        {
            switch (_riskFactorsDataHandler.CheckAssessmentState())
            {
                case AssessmentState.New: LoadIntroPage(); break;
                case AssessmentState.Continuing: RequestQuestionnaire(); break;
                case AssessmentState.Completed: LoadOutroPage(); break;
                default: Logger.LogError("Unknown assessment state"); break;
            }
        }
        
        /// <summary>
        /// Called when the intro page is completed. Initializes assessment data and continues the flow.
        /// </summary>
        
        private void OnIntroCompleted()
        {
            _riskFactorsDataHandler.CreateAssessmentData();
            
            CheckAssessmentProgress();
        }
        
        /// <summary>
        /// Requests the next questionnaire from the questionnaire service.
        /// Broadcasts a callback to be triggered upon completion.
        /// </summary>
       
        private void RequestQuestionnaire()
        {
            if (!_riskFactorsDataHandler.HasData())
            {
                Logger.LogError("No assessment data found");
                return;
            }
            
            var nextAssessmentId = _riskFactorsDataHandler.GetAssessmentId();

            if (string.IsNullOrEmpty(nextAssessmentId))
            {
                Logger.LogError("Invalid assessment ID");
                return;
            }
            
            //Makes a request to the questionnaire builder to load a questionnaire.
            //Passes a call back that is triggered when the questionnaire is completed
            MessageBus.Broadcast<string, IDocument, Action>(QuestionnaireService.OnRequestAssessmentQuestionnaireMessage, nextAssessmentId, this, OnFinishedQuestionnaire);
        }
        
        
        /// <summary>
        /// Callback invoked when a questionnaire is completed.
        /// Updates progress and requests the next step in the assessment.
        /// </summary>
       
        private void OnFinishedQuestionnaire()
        {
            _riskFactorsDataHandler.IncrementProgressIndex();
            
            if (_riskFactorsDataHandler.CheckForEndAssessment())
            {
                LoadOutroPage();
                return;
            }
            
            RequestQuestionnaire();
        }

        /// <summary>
        /// Handles completion of the assessment.
        /// Deletes local assessment data and returns to the navigation screens.
        /// </summary>
       
        private void OnAssessmentCompleted()
        {
            _riskFactorsDataHandler.DeleteAssessmentData();
            
            CloseInfoPages();
            
            OpenNav();
        }
        
        /// <summary>
        /// Opens the navigation screen.
        /// </summary>
       
        private void OpenNav() => MessageBus.Broadcast( nameof(DocumentServiceMessages.OnRequestOpenDocument), DocumentID.Nav);
        
        
        //Pages Loader helpers
        private void LoadIntroPage() => OpenPage(PageID.RiskFactorsIntro);
        private void LoadContinuePage() => OpenPage(PageID.RiskFactorsContinue);
        private void LoadOutroPage() => OpenPage(PageID.RiskFactorsOutro);


        /// <summary>
        /// Closes all active info pages and removes the document root from hierarchy.
        /// </summary>

        private void CloseInfoPages()
        {
            foreach (var page in ActivePages)
                page.Value.Close();
            
            DocumentRoot.RemoveFromHierarchy();
            
            ActivePages.Clear();
        }
    }
}
