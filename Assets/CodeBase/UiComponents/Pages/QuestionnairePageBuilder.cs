using System.Collections.Generic;
using CodeBase.Documents;
using CodeBase.Documents.DemoA;
using CodeBase.Questionnaires;
using CodeBase.UiComponents.Footers;
using CodeBase.UiComponents.Headers;
using CodeBase.UiComponents.Styles;
using ToolBox.Utils;
using UiFrameWork.Components;
using UiFrameWork.Helpers;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Pages
{
    public class QuestionnairePageBuilder
    {
        private const string MainContainerStyle = "fullscreen-container";
        private readonly int _questionCount;
        private ScrollView _scrollView;

        private readonly List<VisualElement> _questions = new();

        public QuestionnairePageBuilder(StandardQuestionnaireTemplate questionnaireData, VisualElement parent)
        {
            _questionCount = questionnaireData.Questions.Length;
            
            Build(questionnaireData, parent);
        }
        private void Build(StandardQuestionnaireTemplate questionnaireTemplate, VisualElement parent)
        {
            var documentRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.DocumentRoot).AttachTo(parent).Build();
            
            //Build the container
            var pageRoot = new ContainerBuilder().AddClass(MainContainerStyle).AttachTo(documentRoot).Build();
            
            //Build the header
            new DefaultHeader("Main Hub", pageRoot, () => { Logger.Log($"OnBack Selected"); }, () => { Logger.Log($"OnClose Selected");  });
            
            //Build the content 
            //Build content
            var content = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(pageRoot).Build();

            _scrollView = new ScrollViewBuilder().EnableInertia(true).SetPickingMode(PickingMode.Position)
                .AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(content).Build();


            var answers = questionnaireTemplate.Answers;
            
            //use question builder to add questions to the scroll view
         
            
            for (int i = 0; i < _questionCount; i++)
            {
                var questionText = questionnaireTemplate.Questions[i];
                
               var question =  new Question(i, questionText, answers, _scrollView.contentContainer, HandleAnswer);
               
               _questions.Add(question.RootElement);
               
            }
            

            //Build the footer
            new SingleButtonFooter(() => { Logger.Log( $"Button clicked" ); }, $"Submit", pageRoot );
        }

        private void HandleAnswer(int questionNumber, string answerText)
        {
            Logger.Log($"OnBack Selected: {questionNumber} - {answerText}");
            
            int nextQuestionNumber = questionNumber + 1;
            
            if (nextQuestionNumber < _questionCount)
                ScrollViewHelper.JumpToElement( _scrollView, _questions[nextQuestionNumber] );
            
           
        }

        public PageID PageIdentifier { get; set; }
    }
}
