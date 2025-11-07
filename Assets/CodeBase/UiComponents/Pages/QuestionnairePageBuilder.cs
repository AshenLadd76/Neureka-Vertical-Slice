using System;
using System.Collections.Generic;
using CodeBase.Documents;
using CodeBase.Documents.DemoA;
using CodeBase.Questionnaires;
using CodeBase.Services;
using CodeBase.UiComponents.Footers;
using CodeBase.UiComponents.Headers;
using CodeBase.UiComponents.Styles;
using ToolBox.Helpers;
using ToolBox.Messenger;
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
        private ScrollView _scrollview;
        
        private VisualElement _root;

        private readonly List<Question> _builtQuestionsList = new();
        
        private Dictionary<int, AnswerData> _answerDictionary = new();

        private StandardQuestionnaireTemplate _questionnaireData;
        
        private ISerializer _jsonSerializer;
        
        
        public QuestionnairePageBuilder(StandardQuestionnaireTemplate questionnaireData, VisualElement root, ISerializer jsonSerializer)
        {
            if (questionnaireData == null)
                throw new ArgumentNullException(nameof(questionnaireData), "Questionnaire data cannot be null.");

            if (questionnaireData.Questions == null || questionnaireData.Questions.Length == 0)
                throw new ArgumentException("Questionnaire must contain at least one question.", nameof(questionnaireData));

            _root = root ?? throw new ArgumentNullException(nameof(root), "Parent VisualElement cannot be null.");

            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer), "JSON serializer cannot be null.");

            _questionCount = questionnaireData.Questions.Length;
            
            _questionnaireData = questionnaireData;
            
            InitializeAnswerDictionary(questionnaireData);
            
            Build(questionnaireData, _root);
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
            
            //Build the content container
            var content = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(pageRoot).Build();

            //Build the scrollview and add it to the content container
            _scrollview = new ScrollViewBuilder().EnableInertia(true).SetPickingMode(PickingMode.Position)
                .AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(content).Build();
            
            var answers = questionnaireTemplate.Answers;
            
            //use question builder to add questions to the scroll view
            CreateAndAddQuestionsToScrollView(questionnaireTemplate, answers, _scrollview);
            
            CreateFooter(pageRoot);
        }

        private void CreateAndAddQuestionsToScrollView(StandardQuestionnaireTemplate questionnaireTemplate, string[] answers, ScrollView scrollView)
        {
            for (int i = 0; i < _questionCount; i++)
            {
                var questionText = questionnaireTemplate.Questions[i];

                var question = QuestionFactory.BuildQuestion(i, questionText, answers,HandleAnswer, scrollView.contentContainer);
                
                _builtQuestionsList.Add(question);
            }
        }

        private void CreateFooter(VisualElement parent)
        {
            //Build the footer
            new SingleButtonFooter(() =>
            {
                if (!QuestionnaireValidator.ValidateAnswers(_builtQuestionsList, _scrollview))
                {
                    Logger.LogWarning("answers incomplete - failed validation.");
                    return;
                }
                
                HandleSubmit();
                
            }, $"Submit", parent );
            
        }

        private void HandleAnswer(int questionIndex, string answerText)
        {
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

        private void HandleSubmit()
        {
            try
            {
                var questionnaireData = new QuestionnaireDataBuilder().SetTemplate(_questionnaireData)
                    .SetAnswers(_answerDictionary).Build();
                
                var jsonData = _jsonSerializer.Serialize(questionnaireData);

                var webData = new WebData
                {
                    Id = _questionnaireData.QuestionnaireID,
                    Data = jsonData
                };
                
                MessageBus.Instance.Broadcast( NeurekaDemoMessages.DataUploadRequestMessage, webData );
                
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to handle submit: {e}");
            }
        }
        
        
        //Helper function that is will be attached to the relevant button events
        //ensures the root visual element is cleared when the questionnaires life is ended
        private void Clear()
        {
            _root.Clear();
        }
    }
}
