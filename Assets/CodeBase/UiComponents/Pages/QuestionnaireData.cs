using System;
using System.Collections.Generic;

namespace CodeBase.UiComponents.Pages
{
    [Serializable]
    public class QuestionnaireData
    {
        public string PlayerId { get; set; }
        public string ScientificId { get; set; }
        public string ScientificName { get; set; }
        public string AssessmentId { get; set; }
        public string QuestionnaireID { get; set; }
        public string QuestionnaireName { get; set; }
        public string QuestionnaireDescription { get; set; }
        public Dictionary<int, AnswerData> Answers { get; set; }
        public List<int> ReverseScored { get; set; }
    }
}