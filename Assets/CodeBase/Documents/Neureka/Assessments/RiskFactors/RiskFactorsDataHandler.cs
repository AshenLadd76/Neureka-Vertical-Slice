using System.Collections.Generic;
using ToolBox.Extensions;
using ToolBox.Services.Data;
using ToolBox.Utils;

namespace CodeBase.Documents.Neureka.Assessments.RiskFactors
{
    public class RiskFactorsDataHandler
    {
        private const string Directory = "RiskFactors";
        private const string FileName = "RiskFactorsData.json";
        
        private readonly IFileDataService _fileDataService;
        
        private RiskFactorsData _riskFactorsData;
        
        private AssessmentState _currentAssessmentState;
        
        
        public RiskFactorsDataHandler(IFileDataService fileDataService)
        {
            _fileDataService = fileDataService;
        }
        
        public void CreateAssessmentData()
        {
            if (CheckDataExists())
            {
                LoadRiskFactorsData();
                
                Logger.Log( $"Data already exists so im skipping it " );
                return;
            }

            _riskFactorsData = new RiskFactorsData
            {
                //Demo questionnaires
                AssessmentIdList = new List<string>
                {
                    "HHI",
                    "AQ",
                    "SQS",
                    "DASS",
                    "MCQ-30",
                    "HRSI"
                }
            };
            
            _riskFactorsData.AssessmentIdList.Shuffle();
            
            _fileDataService.Save(_riskFactorsData, Directory, FileName, false, true);
        }
        
        private bool CheckDataExists() => _fileDataService.FileExists(Directory, FileName).Value;
        
        public bool CheckForEndAssessment() => _riskFactorsData == null || _riskFactorsData.ProgressIndex >= _riskFactorsData.AssessmentIdList.Count;
        
        private void LoadRiskFactorsData() => _riskFactorsData = _fileDataService.Load<RiskFactorsData>(Directory, FileName).Value;
        

        public void DeleteAssessmentData()
        {
            _fileDataService.Delete(Directory, FileName);
            
            _riskFactorsData = null;
            
            _currentAssessmentState = AssessmentState.New;
        }
        
        public void IncrementProgressIndex()
        {
            _riskFactorsData ??= _fileDataService.Load<RiskFactorsData>(Directory, FileName, false).Value;
            
            // Advance progress
            if (!CheckForEndAssessment())
                _riskFactorsData.ProgressIndex += 1;
            
            // Persist updated progress
            _fileDataService.Save(_riskFactorsData, Directory, FileName, false);
        }

        public string GetAssessmentId()
        {
            if( _riskFactorsData == null ) LoadRiskFactorsData();
            
            return _riskFactorsData?.AssessmentIdList[_riskFactorsData.ProgressIndex];
        }

        public AssessmentState CheckAssessmentState()
        {
            if (_riskFactorsData == null && CheckDataExists())
                LoadRiskFactorsData();

            if (_riskFactorsData == null)
                _currentAssessmentState = AssessmentState.New;
            else if (_riskFactorsData.ProgressIndex >= _riskFactorsData.AssessmentIdList.Count)
                _currentAssessmentState = AssessmentState.Completed;
            else
                _currentAssessmentState = AssessmentState.Continuing;
            
            return _currentAssessmentState;
        }
        
        public bool HasData() => _riskFactorsData != null;
    }
}