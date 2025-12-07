using System;
using System.Collections.Generic;
using CodeBase.Services;
using ToolBox.Messenger;
using ToolBox.Services.Data;
using UiFrameWork.RunTime;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Assessments
{
    public class RiskFactorsDocument : BaseDocument
    {
        private readonly RiskFactorsDataHandler _riskFactorsDataHandler;
        
        public RiskFactorsDocument(IFileDataService fileDataService)
        {
            _riskFactorsDataHandler = new RiskFactorsDataHandler(fileDataService);
        }
        
        protected override void Build()
        {
            base.Build();
            AddPageRecipes();
            CheckAssessmentStatus();
        }
        
        private void AddPageRecipes()
        {
            string title = "Risk Factors";
            
            var introContentList = new List<BlurbContent>
            {
                new("001",  $"Thanks for clicking on the Risk Factors  science challenge!\n\nThis challenge is all about helping researchers find new ways to detect dementia early.", $"Assessments/RiskFactors/Images/intro_image_1"),
                new("002", $"You'll be asked to complete 6 questionnaires about yourself. You can complete all questionnaires in one go or complete them one by one, over several days. \n\nYour progress will be saved each time you finish a questionnaire.", "Assessments/RiskFactors/Images/intro_image_2"),
                
            };
            var introContent = new IntroPageContent( title, introContentList, OnFinishedIntro );
            
            
            PageRecipes[PageID.RiskFactorsIntro] = () => new IntroPage(this, introContent);
            PageRecipes[PageID.RiskFactorsOutro] = () => new IntroPage(this, introContent);
        }
        
        private void CheckAssessmentStatus()
        {
            switch (_riskFactorsDataHandler.CheckAssessmentState())
            {
                case AssessmentState.New: LoadIntro(); break;
                case AssessmentState.Continuing: RequestQuestionnaire(); break;
                case AssessmentState.Completed: LoadOutro(); break;
                default: Logger.LogError("Unknown assessment state"); break;
            }
        }
        
        private void OnFinishedIntro()
        {
            _riskFactorsDataHandler.CreateAssessmentData();
            
            CheckAssessmentStatus();
        }
        
        private void RequestQuestionnaire()
        {
            if (!_riskFactorsDataHandler.HasData())
            {
                Logger.LogError("No assessment data found");
                return;
            }
            
            var nextAssessmentId = _riskFactorsDataHandler.GetAssessmentId();
            
            MessageBus.Instance.Broadcast<string, IDocument, Action>(QuestionnaireService.OnRequestAssessmentQuestionnaireMessage, nextAssessmentId, this, OnFinishedQuestionnaire);
        }

        
        private void OnFinishedQuestionnaire()
        {
            _riskFactorsDataHandler.IncrementProgressIndex();
            
            if (_riskFactorsDataHandler.CheckForEndAssessment())
            {
                Logger.Log( $"We are finished the assessment......." );
                LoadOutro();
                return;
            }
            
            RequestQuestionnaire();
        }
        
        private void LoadIntro() => OpenPage(PageID.RiskFactorsIntro);
        private void LoadOutro() => OpenPage(PageID.RiskFactorsOutro);

        public override void Close()
        {
            Logger.Log( $"Closing Risk Factors Document" );
            
            base.Close();
        }
    }
}
