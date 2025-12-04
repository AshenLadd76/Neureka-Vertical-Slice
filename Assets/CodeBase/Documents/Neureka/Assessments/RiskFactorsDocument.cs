using System;
using System.Collections.Generic;
using CodeBase.Services;
using ToolBox.Extensions;
using ToolBox.Messenger;
using ToolBox.Services.Data;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Assessments
{
    public class RiskFactorsDocument : BaseDocument
    {
        private const string Directory = "RiskFactors";
        private const string FileName = "RiskFactorsData.json";
        
        private VisualElement _documentRoot;
        
        private AssessmentState _assessmentState;

        private int _progressIndex = -1;
        private int _blurbContentCount;
        
        private readonly IFileDataService _fileDataService;
        
        private RiskFactorsData _riskFactorsData;
        
        protected override void Build()
        {
            base.Build();
            
            AddPageRecipes();
            
            var doesFileExist = _fileDataService.FileExists(Directory, FileName);
            
            CreateAssessmentData(doesFileExist);
             
            _assessmentState = !doesFileExist ? AssessmentState.New : AssessmentState.Continuing;
            
            CheckAssessmentStatus();
        }
        

        public RiskFactorsDocument(IFileDataService fileDataService)
        {
            _fileDataService = fileDataService;
        }
        
        private void AddPageRecipes()
        {
            string title = "Risk Factors";
            
            var introContentList = new List<BlurbContent>
            {
                new BlurbContent("001",  $"Thanks for clicking on the Risk Factors  science challenge!\n\nThis challenge is all about helping researchers find new ways to detect dementia early.", "Riskfactors/Images/blurb001"),
                new BlurbContent("002", $"You'll be asked to complete 6 questionnaires about yourself. You can complete all questionnaires in one go or complete them one by one, over several days. \n\nYour progress will be saved each time you finish a game or questionnaire.", "Riskfactors/Images/blurb002"),
                
            };
            var introContent = new IntroPageContent( title, introContentList, OnFinishedIntro );
            
            
            PageRecipes[PageID.RiskFactorsIntro] = () => new IntroPage(this, introContent);
            PageRecipes[PageID.RiskFactorsOutro] = () => new IntroPage(this, introContent);
        }

        
        private void CheckAssessmentStatus()
        {
            switch (_assessmentState)
            {
                case AssessmentState.New: LoadIntro(); break;
                case AssessmentState.Continuing: RequestQuestionnaire(); break;
                case AssessmentState.Completed: LoadOutro(); break;
                default: Logger.LogError("Unknown assessment state"); break;
            }
        }

      
        
        private void CreateAssessmentData(bool doesDataExist)
        {
            if (doesDataExist)
            {
                _riskFactorsData = _fileDataService.Load<RiskFactorsData>(Directory, FileName).Value;
                _progressIndex = _riskFactorsData.ProgressIndex;
                
                Logger.Log( $"Data already exists so im skipping it " );
                return;
            }

            _riskFactorsData = new RiskFactorsData
            {
                AssessmentIdList = new List<string>
                {
                    "hhi-10",
                    "AQ",
                    "cesd-20",
                    "sqs-30",
                    "hhi-10",
                    "cesd-20"
                }
            };
            
            _riskFactorsData.AssessmentIdList.Shuffle();
            
            _fileDataService.Save(_riskFactorsData, Directory, FileName, false);
        }
        
        private void OnFinishedIntro()
        {
            _assessmentState = AssessmentState.Continuing;
            _progressIndex = 0;
            
            CheckAssessmentStatus();
        }
        
        private void IncrementProgressIndex()
        {
            _riskFactorsData ??= _fileDataService.Load<RiskFactorsData>(Directory, FileName, false).Value;
            
            // Advance progress
            _progressIndex =  Math.Clamp(_riskFactorsData.ProgressIndex + 1, 0, _riskFactorsData.AssessmentIdList.Count);
            _riskFactorsData.ProgressIndex = _progressIndex;

            // Persist updated progress
            _fileDataService.Save(_riskFactorsData, Directory, FileName, false);
        }

        private void RequestQuestionnaire()
        {
            var nextAssessmentId = _riskFactorsData?.AssessmentIdList[_progressIndex];
            
            MessageBus.Instance.Broadcast<string, Action>(QuestionnaireService.OnRequestAssessmentQuestionnaireMessage, nextAssessmentId, OnFinishedQuestionnaire);
        }

        private bool CheckForEndAssessment() => _progressIndex >= _riskFactorsData?.AssessmentIdList.Count - 1;

        private void OnFinishedQuestionnaire()
        {
            IncrementProgressIndex();
            
            if (CheckForEndAssessment())
            {
                Logger.Log( $"We are finished the assessment......." );
                LoadOutro();
                return;
            }
            
            RequestQuestionnaire();
        }
        
        private void LoadIntro() => OpenPage(PageID.RiskFactorsIntro);
        private void LoadOutro() => OpenPage(PageID.RiskFactorsOutro);
    }
}
