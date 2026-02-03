using System.Collections.Generic;
using System.Linq;
using CodeBase.Questionnaires;

namespace CodeBase.UiComponents.Pages
{
    public class QuestionnaireDataBuilder
    {
        private StandardQuestionnaireTemplate _template;
        private Dictionary<int, AnswerData>  _answers = new();

        public QuestionnaireDataBuilder SetTemplate(StandardQuestionnaireTemplate template)
        {
            _template = template;
            return this;
        }

        public QuestionnaireDataBuilder SetAnswers(Dictionary<int, AnswerData> answers)
        {
            _answers = answers;
            return this;
        }

        public QuestionnaireData Build()
        {
            if (_template == null)
                throw new System.InvalidOperationException("Questionnaire template must be set.");
            if (_answers == null)
                throw new System.InvalidOperationException("Answer dictionary must be set.");

            return new QuestionnaireData
            {
                PlayerId = _template.PlayerId,
                ScientificId = _template.ScientificId,
                ScientificName = _template.ScientificName,
                AssessmentId = _template.AssessmentId,
                QuestionnaireID = _template.QuestionnaireID,
                QuestionnaireName = _template.QuestionnaireName,
                QuestionnaireDescription = _template.QuestionnaireDescription,
                Answers = _answers,
                ReverseScored = _template.ReverseScored.ToList()
            };
        }
    }
}