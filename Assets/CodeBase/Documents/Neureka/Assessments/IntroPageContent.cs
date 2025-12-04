using System;
using System.Collections.Generic;

namespace CodeBase.Documents.Neureka.Assessments
{
    [Serializable]
    public class IntroPageContent
    {
        public IntroPageContent(string title, List<BlurbContent> blurbContentList, Action onFinished)
        {
            Title = title;
            ContentList = blurbContentList;
            OnFinished = onFinished;
        }
        
        public string Title { get; set; }
        public List<BlurbContent> ContentList { get; set; }
        
        public Action OnFinished { get; set; }
    }
}