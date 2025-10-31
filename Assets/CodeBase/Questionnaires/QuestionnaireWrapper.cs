using System;

namespace CodeBase.Questionnaires
{
    [Serializable]
    public class QuestionnaireWrapper
    {
        public MetaData MetaData { get; set; }
        public StandardQuestionnaireTemplate Questionnaire { get; set; }
    }
}