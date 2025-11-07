using System;

namespace CodeBase.UiComponents.Pages
{
    [Serializable]
    public class AnswerData
    {
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public DateTime Timestamp { get; set; }
        
        public AnswerData(int questionNumber, string questionText, string answerText)
        {
            QuestionNumber = questionNumber;
            QuestionText = questionText;
            AnswerText = answerText;
            Timestamp = DateTime.UtcNow;
        }
    }
}

