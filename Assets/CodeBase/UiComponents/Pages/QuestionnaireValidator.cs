using System.Collections.Generic;
using FluentUI.Helpers;
using ToolBox.Extensions;
using ToolBox.Utils;
using UiFrameWork.Helpers;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Pages
{
    public static class QuestionnaireValidator
    {
        
        private const string ValidationOutline = "unanswered-highlight";
        
        public static bool ValidateAnswers(List<Question> questions, ScrollView scrollView)
        {
            if (questions.IsNullOrEmpty())
            {
                Logger.LogError("Questions are empty");
                return false;
            }

            if (scrollView == null)
            {
                Logger.LogError("ScrollView is null");
            }
            
            bool jumped = false;

            bool passedValidation = true;
            
            // get list of all questions
            foreach (var question in questions)
            {
                if (!question.IsAnswered)
                {
                    if( !jumped )
                        ScrollViewHelper.JumpToElementSmooth( scrollView, question.RootVisualElement );
                    
                    question.ToggleWarningOutline(true);
                    passedValidation = false;
                    
                    jumped = true;
                }
                else
                {
                    question.ToggleWarningOutline(false);
                }
            }

            return passedValidation;
            
        }
    }
}