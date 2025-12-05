using System.Collections.Generic;
using ToolBox.Extensions;
using ToolBox.Services.Data;
using ToolBox.Utils;

namespace CodeBase.Documents.Neureka.Assessments
{
    public class RiskFactorsDataHandler
    {
        private const string Directory = "RiskFactors";
        private const string FileName = "RiskFactorsData.json";
        
        private readonly IFileDataService _fileDataService;
        
        private RiskFactorsData _riskFactorsData;
        
        public RiskFactorsData RiskFactorsData
        {
            get => _riskFactorsData;
            set => _riskFactorsData = value;
        }

        private int _progressIndex = 0;
        
        public RiskFactorsDataHandler(IFileDataService fileDataService)
        {
            _fileDataService = fileDataService;
            
        }
        
        public void CreateAssessmentData()
        {
            Logger.Log( $"Creating assessment data..." );
            
            _progressIndex = 0;
            
            if (CheckDataExists())
            {
                LoadRiskFactorsData();
                
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
        
        public bool CheckDataExists() => _fileDataService.FileExists(Directory, FileName);
        
        public bool CheckForEndAssessment() => _progressIndex >= _riskFactorsData?.AssessmentIdList.Count;
        
        public void LoadRiskFactorsData()
        {
            _riskFactorsData = _fileDataService.Load<RiskFactorsData>(Directory, FileName).Value;
            _progressIndex = _riskFactorsData.ProgressIndex;
        }
        
        public void IncrementProgressIndex()
        {
            _riskFactorsData ??= _fileDataService.Load<RiskFactorsData>(Directory, FileName, false).Value;
            
            // Advance progress
            if (!CheckForEndAssessment())
                _progressIndex = _riskFactorsData.ProgressIndex + 1;
                
            
            _riskFactorsData.ProgressIndex = _progressIndex;

            // Persist updated progress
            _fileDataService.Save(_riskFactorsData, Directory, FileName, false);
        }

        public string GetAssessmentId()
        {
            if( _riskFactorsData == null ) LoadRiskFactorsData();
            
            return _riskFactorsData?.AssessmentIdList[_progressIndex];
        }

        public AssessmentState CheckAssessmentState()
        {
            if (_riskFactorsData == null && CheckDataExists())
                LoadRiskFactorsData();
    
            if (_riskFactorsData != null && _progressIndex >= _riskFactorsData.AssessmentIdList.Count)
                return AssessmentState.Completed;

            return CheckDataExists() ? AssessmentState.Continuing : AssessmentState.New;
        }
        
        public bool HasData() => _riskFactorsData != null;
        
    }
}