using System;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Pages
{
    public static class QuestionFactory
    {
        public static Question BuildQuestion(
            int index,
            string questionText,
            string[] answers,
            Action<int, string> onOptionSelected,
            VisualElement parent = null)
        {
            var question =  new Question()
                .SetIndex(index)
                .SetMultiSelection(false)
                .SetQuestionText(questionText)
                .SetAnswers(answers)
                .SetOnOptionSelected(onOptionSelected)
                .AddLabelClass("question-container-label")
                .AddContainerClass("question-container")
                .Build();
            
            if( parent != null )  question.AttachTo(parent);
            
            return question;
        }
    }
}