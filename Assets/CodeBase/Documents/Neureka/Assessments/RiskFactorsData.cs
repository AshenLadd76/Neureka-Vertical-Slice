using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace CodeBase.Documents.Neureka.Assessments
{
    [Serializable]
    public class RiskFactorsData
    {
        
        private List<string> _assessmentIdList;
        public List<string> AssessmentIdList
        {
            get => _assessmentIdList;
            set => _assessmentIdList = value;
        }
        
        public int ProgressIndex = -1;

        
        public RiskFactorsData()
        {
            
        }

        public static class RiskFactorsContent
        {
            public const string HHI = "hhi";
            public const string DFH = "dfh";
            public const string ATOA = "atoa";
            public const string SDS = "sds";
            public const string SES = "ses";
            public const string UCLA = "ucla";
        }
    }
}