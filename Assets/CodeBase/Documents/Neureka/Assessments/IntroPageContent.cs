using System;
using System.Collections.Generic;
using CodeBase.Documents.Neureka.Assessments.RiskFactors;

namespace CodeBase.Documents.Neureka.Assessments
{
    [Serializable]
    public class InfoPageContent
    {
        public InfoPageContent(string title, string buttonText, List<BlurbContent> blurbContentList, Action onFinished = null)
        {
            Title = title;
            ButtonText = buttonText;
            ContentList = blurbContentList;
            OnFinished = onFinished;
        }
        
        public string Title { get; set; }
        public string ButtonText { get; set; }
        public List<BlurbContent> ContentList { get; set; }
        
        public Action OnFinished { get; set; }
    }
}