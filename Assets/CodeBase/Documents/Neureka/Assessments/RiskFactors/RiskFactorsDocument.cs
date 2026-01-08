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
    /// 
    /// This document acts as the orchestration layer between UI pages,
    /// persisted assessment state, and the questionnaire service.
    /// </summary>
    
    public class RiskFactorsDocument : BaseDocument
    {
        private  RiskFactorsDataHandler _riskFactorsDataHandler;
        
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

        
        public override void Build(VisualElement root)
        {
            base.Build(root);

            if (!LoadRiskFactorsSo())
            {
                Logger.LogError("RiskFactorsSO could not be loaded!");
                return;
            }

            AddPageRecipes();
            
            //Open( root );
        }
        
        public override void Open(VisualElement visualElement)
        {
            //Open a welcome back/ continue page when previous progress exists.
            if (_riskFactorsDataHandler.CheckAssessmentState() == AssessmentState.Continuing)
                LoadContinuePage();
            else
                CheckAssessmentProgress();
        }
        
        
        private void AddPageRecipes()
        {
            CreatePage(PageID.RiskFactorsIntro, _riskFactorsSo.IntroTitle, _riskFactorsSo.IntroButtonText, _riskFactorsSo.IntroBlurbContents, OnIntroCompleted);
            CreatePage(PageID.RiskFactorsContinue, _riskFactorsSo.ContinueTitle, _riskFactorsSo.ContinueButtonText, _riskFactorsSo.ContinueBlurbContents, CheckAssessmentProgress);
            CreatePage(PageID.RiskFactorsOutro, _riskFactorsSo.OutroTitle, _riskFactorsSo.OutroButtonText, _riskFactorsSo.OutroBlurbContents, OnAssessmentCompleted);
        }

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
        
        private void CreatePage(PageID pageId, string title, string buttonText, List<BlurbContent> blurbs, Action onCompleted = null)
        {
            if (blurbs.IsNullOrEmpty())
            {
                Logger.LogError($"RiskFactorsSO list for {pageId} is empty or null!");
                return;
            }

            var content = new InfoPageContent(title, buttonText, blurbs, onCompleted);
            
           // PageRecipes[pageId] = () => new InfoPage(this, content);
           
           ActivePages[pageId] = new InfoPage(this, content);
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
        
        private void OnIntroCompleted()
        {
            _riskFactorsDataHandler.CreateAssessmentData();
            
            CheckAssessmentProgress();
        }
        
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
            
            MessageBus.Broadcast<string, IDocument, Action>(QuestionnaireService.OnRequestAssessmentQuestionnaireMessage, nextAssessmentId, this, OnFinishedQuestionnaire);
        }
        
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

        private void OnAssessmentCompleted()
        {
            _riskFactorsDataHandler.DeleteAssessmentData();
            
            CloseInfoPages();
            
            OpenHub();
        }
        
        private void OpenHub() => MessageBus.Broadcast( nameof(DocumentServiceMessages.OnRequestOpenDocument), DocumentID.Neureka);
        
        private void LoadIntroPage() => OpenPage(PageID.RiskFactorsIntro);
        private void LoadContinuePage() => OpenPage(PageID.RiskFactorsContinue);
        private void LoadOutroPage() => OpenPage(PageID.RiskFactorsOutro);


        private void CloseInfoPages()
        {
            foreach (var page in ActivePages)
                page.Value.Close();
        }
    }
}
