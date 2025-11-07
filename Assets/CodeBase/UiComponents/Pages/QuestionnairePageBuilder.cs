using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Documents;
using CodeBase.Documents.DemoA;
using CodeBase.Questionnaires;
using CodeBase.UiComponents.Footers;
using CodeBase.UiComponents.Headers;
using CodeBase.UiComponents.Styles;
using Newtonsoft.Json;
using ToolBox.Utils;
using UiFrameWork.Components;
using UiFrameWork.Helpers;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Pages
{
    public class QuestionnairePageBuilder
    {
        private const string MainContainerStyle = "fullscreen-container";
        private int _questionCount;
        private ScrollView _scrollview;

        private readonly List<Question> _builtQuestionsList = new();
        
        private Dictionary<int, AnswerData> _answerDictionary = new();

        private StandardQuestionnaireTemplate _questionnaireData;
        
        
        public QuestionnairePageBuilder(StandardQuestionnaireTemplate questionnaireData, VisualElement parent)
        {
            if (questionnaireData == null)
                throw new ArgumentNullException(nameof(questionnaireData), "Questionnaire data cannot be null.");

            if (questionnaireData.Questions == null || questionnaireData.Questions.Length == 0)
                throw new ArgumentException("Questionnaire must contain at least one question.", nameof(questionnaireData));

            if (parent == null)
                throw new ArgumentNullException(nameof(parent), "Parent VisualElement cannot be null.");

            _questionCount = questionnaireData.Questions.Length;
            
            _questionnaireData = questionnaireData;
            
            InitializeAnswerDictionary(questionnaireData);
            Build(questionnaireData, parent);
        }

        private void InitializeAnswerDictionary(StandardQuestionnaireTemplate questionnaireData)
        {
            for (int x = 0; x < _questionCount; ++x)
            {
                int questionNumber = x + 1;
                string questionText = questionnaireData.Questions[x];
               
                _answerDictionary.Add(questionNumber, new AnswerData(questionNumber, questionText, "" ));
            }
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

            _scrollview = new ScrollViewBuilder().EnableInertia(true).SetPickingMode(PickingMode.Position)
                .AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(content).Build();


            var answers = questionnaireTemplate.Answers;
            
            //use question builder to add questions to the scroll view
         
            
            for (int i = 0; i < _questionCount; i++)
            {
                var questionText = questionnaireTemplate.Questions[i];

                var question = new Question().SetIndex(i)
                    .SetMultiSelection(false)
                    .SetQuestionText(questionText)
                    .SetAnswers(answers)
                    .SetOnOptionSelected(HandleAnswer)
                    .AddLabelClass("question-container-label")
                    .AddContainerClass("question-container")
                    .AttachTo(_scrollview.contentContainer)
                    .Build(); 
                
                //new Question(i, questionText, answers, _scrollview.contentContainer, HandleAnswer);
               
               _builtQuestionsList.Add(question);
            }
            
            //Build the footer
            new SingleButtonFooter(() =>
            {
                if (!QuestionnaireValidator.ValidateAnswers(_builtQuestionsList, _scrollview))
                {
                    Logger.Log("answers incomplete");
                    return;
                }
                
                Logger.Log( $"Button clicked" );
                Submit();
            }, $"Submit", pageRoot );
            
            
        }

        private void HandleAnswer(int questionIndex, string answerText)
        {
            Logger.Log($"OnBack Selected: {questionIndex} - {answerText}");
            
            SetAnswer(questionIndex+1, answerText);
            
            int nextQuestionNumber = questionIndex + 1;
            
            if (nextQuestionNumber < _questionCount)
                ScrollViewHelper.JumpToElementSmooth( _scrollview, _builtQuestionsList[nextQuestionNumber].RootVisualElement );

            _builtQuestionsList[questionIndex].ToggleWarningOutline(false);

        }

        private void SetAnswer(int questionNumber, string answerText)
        {
            if (!_answerDictionary.TryGetValue(questionNumber, out var value))
            {
                Logger.LogError($"Attempted to set answer for invalid question number {questionNumber}");
                return;
            }
     
            value.AnswerText = answerText;
        }

        private void Submit()
        {
            if (_answerDictionary.Values.Any(a => string.IsNullOrEmpty(a.AnswerText)))
            {
                Logger.LogWarning("Cannot submit: Some questions are unanswered.");
                QuestionnaireValidator.ValidateAnswers(_builtQuestionsList, _scrollview);
                return;
            }

            try
            {
                Logger.Log( $"Checking answers dictionary >> { _answerDictionary.Count }" );
                
                var questionnaireData = new QuestionnaireDataBuilder().SetTemplate(_questionnaireData)
                    .SetAnswers(_answerDictionary).Build();
               
            
                var json = JsonConvert.SerializeObject(questionnaireData);
                
                Logger.Log( json );
            
                //TODO: Implement Serialisation service
            
                //TODO: Send to DataUploader Service
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public PageID PageIdentifier { get; set; }
    }
    
        public class QuestionnaireDataBuilder
        {
            private StandardQuestionnaireTemplate _template;
            private Dictionary<int, AnswerData> _answers = new();

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
                    QuestionnaireDescription = _template.QuestionnaireName,
                    Answers = _answers,
                    ReverseScored = _template.ReverseScored.ToList()
                };
            }
        }
        
}
