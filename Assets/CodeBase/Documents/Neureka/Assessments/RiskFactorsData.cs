using System;
using System.Collections.Generic;

namespace CodeBase.Documents.Neureka.Assessments
{
    [Serializable]
    public class RiskFactorsData
    {
        
        private List<string> assessmentIdList = new();
        public List<string> AssessmentIdList
        {
            get => assessmentIdList;
            set => assessmentIdList = value;
        }
        
        public int progressIndex = -1;

        
        public RiskFactorsData()
        {
            assessmentIdList.Add( "hhi" );
            assessmentIdList.Add( "dfh" );
            assessmentIdList.Add( "atoa" );
            assessmentIdList.Add( "sds" );
            assessmentIdList.Add( "ses" );
            assessmentIdList.Add( "ucla" );
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