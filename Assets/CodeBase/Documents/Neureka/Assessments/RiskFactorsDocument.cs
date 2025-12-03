using System;
using System.Collections.Generic;
using ToolBox.Extensions;
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
        private int _blurbIndex = -1;
        private int _blurbContentCount;
        
        private readonly IFileDataService _fileDataService;


        protected override void Build()
        {
            base.Build();
            
            AddPageRecipes();
            
            var doesFileExist = _fileDataService.FileExists(Directory, FileName);
             
            if (!doesFileExist)
            {
                _assessmentState = AssessmentState.New;
            }
            else
            {
                //Load data and check progress...
            }
            
             
            //Check if assessment data exists
            CheckAssessmentStatus();
            
            CreateAssessmentData();
            
            
        }
        

        public RiskFactorsDocument(IFileDataService fileDataService)
        {
            _fileDataService = fileDataService;
        }
        
        
        private void AddPageRecipes()
        {
            string title = "Risk Factors";
            
            var blurbContentList = new List<BlurbContent>
            {
                new BlurbContent("001",  $"Thanks for clicking on the Risk Factors  science challenge!\n\nThis challenge is all about helping researchers find new ways to detect dementia early.", "Riskfactors/Images/blurb001"),
                new BlurbContent("002", $"You'll be asked to complete 6 questionnaires about yourself. You can complete all questionnaires in one go or complete them one by one, over several days. \n\nYour progress will be saved each time you finish a game or questionnaire.", "Riskfactors/Images/blurb002"),
              
                
            };
            var content = new IntroPageContent( title, blurbContentList );
            
            PageRecipes[PageID.RiskFactorsIntro] = () => new IntroPage(this, content);
        }

        
        private void CheckAssessmentStatus()
        {
            switch (_assessmentState)
            {
                case AssessmentState.New:
                    LoadIntro();
                    break;
                case AssessmentState.Continuing:
                    LoadNextSection();
                    break;
                case AssessmentState.Completed:
                    LoadEndSection();
                    break;
                default:
                    Logger.LogError("Unknown assessment state");
                    break;
            }

        }

        private void LoadIntro()
        {
            Logger.Log( $"Loading intro pages" );
            OpenPage(PageID.RiskFactorsIntro);
        }

        private void CreateAssessmentData()
        {
            if (_fileDataService.FileExists(Directory, FileName)) return;
            
            var assessmentData = new RiskFactorsData();
            assessmentData.AssessmentIdList.Shuffle();
            
            _fileDataService.Save(assessmentData, Directory, FileName, false);
        }

        private void OnFinishedIntro()
        {
            //Create a new riskfactors data object and save it
            
        }

        private void LoadNextSection()
        {
            Logger.Log( $"Loading next section" );
            //Increment progress index;
            
            //Get next questionnaire id
            
            //Request questionnaire from questionnaire builder
            
            //Pass this method as a callback
            
        }

        private void LoadEndSection()
        {
            Logger.Log( $"Loading end section" );
        }
        
    }

    public enum AssessmentState
    {
        New,
        Continuing,
        Completed
    }

    public class BlurbContent
    {
        public BlurbContent(string id, string blurb, string imagePath)
        {
            Id = id;
            Blurb = blurb;
            ImagePath = imagePath;
                
        }
        
        public string Id { get; set; }
        public string Blurb { get; set; }
        public string ImagePath { get; set; }
    }

    [Serializable]
    public class IntroPageContent
    {
        public IntroPageContent(string title, List<BlurbContent> blurbContentList)
        {
            Title = title;
            ContentList = blurbContentList;
            
        }
        
        public string Title { get; set; }
        public List<BlurbContent> ContentList { get; set; }
    }
}
