using System;
using System.Collections.Generic;
using CodeBase.Documents.DemoA;
using CodeBase.Documents.Neureka.Components;
using CodeBase.Questionnaires;
using CodeBase.Services;

using CodeBase.UiComponents.Styles;
using ToolBox.Helpers;
using ToolBox.Messenger;
using ToolBox.Services.Haptics;
using UiFrameWork.Components;
using UiFrameWork.Helpers;

using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.UiComponents.Pages
{
    public class QuestionnairePageBuilder
    {
        private const string MainContainerStyle = "fullscreen-container";
        private readonly int _questionCount;
        private ScrollView _scrollview;
        
        private VisualElement _root;

        private readonly List<Question> _builtQuestionsList = new();
        
        private readonly Dictionary<int, AnswerData> _answerDataDictionary = new();

        private readonly StandardQuestionnaireTemplate _questionnaireData;
        
        private readonly ISerializer _jsonSerializer;
        
        private ProgressBarController _progressBarController;
        
        
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
        }

        private void InitializeAnswerDictionary(StandardQuestionnaireTemplate questionnaireData)
        {
            for (int x = 0; x < _questionCount; ++x)
            {
                int questionNumber = x + 1;
                string questionText = questionnaireData.Questions[x];
               
                _answerDataDictionary.Add(questionNumber, new AnswerData(questionNumber, questionText, "" ));
            }
        }
        
        public void Build()
        {
            var documentRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.DocumentRoot).AttachTo(_root).Build();
            
            //Build the container
            var pageRoot = new ContainerBuilder().AddClass(MainContainerStyle).AttachTo(documentRoot).Build();
            
            CreateHeader(pageRoot);
            
            //Build the content container
            var content = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(pageRoot).Build();

            //Build the scrollview and add it to the content container
            _scrollview = new ScrollViewBuilder().EnableInertia(true).SetPickingMode(PickingMode.Position)
                .AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).Build();
            
            var answers = _questionnaireData.Answers;
            
            //use question builder to add questions to the scroll view
            CreateAndAddQuestionsToScrollView(_questionnaireData, answers, _scrollview);
            
            content.Add(_scrollview);
            
            CreateFooter(pageRoot);
        }

        private void CreateAndAddQuestionsToScrollView(StandardQuestionnaireTemplate questionnaireTemplate, string[] answers, ScrollView scrollView)
        {
            for (int i = 0; i < _questionCount; i++)
            {
                var questionText = questionnaireTemplate.Questions[i];
                
                var question =  new Question()
                    .SetIndex(i)
                    .SetMultiSelection(false)
                    .SetQuestionText(questionText)
                    .SetAnswers(answers)
                    .SetOnOptionSelected(HandleAnswer)
                    .AddLabelClass("question-container-label")
                    .AddContainerClass("question-container")
                    .Build();
                
                _builtQuestionsList.Add(question);
                
                scrollView.contentContainer.Add(question.RootVisualElement);
            }
        }

        private void CreateHeader(VisualElement parent)
        {
            var headerNav = new ContainerBuilder().AddClass("header-nav").AttachTo(parent).Build();
            
            new ButtonBuilder().SetText("X")
                .OnClick(CreateQuitPopUp)
                .AddClass("demo-header-button")
                .AddClass(UiStyleClassDefinitions.HeaderLabel)
                .AttachTo(headerNav)
                .Build();
            
            var headerTitle =  new ContainerBuilder().AddClass("header-title").AttachTo(parent).Build();

            var label = new LabelBuilder().SetText(_questionnaireData.QuestionnaireName).AddClass("header-label").AttachTo(headerTitle).Build();
            
            _progressBarController = new ProgressBarController("progress-bar-header", 1f,_questionCount,parent);
            
            _progressBarController.SetFillAmount(0);
        }
        
        private void CreateQuitPopUp()
        {
            HapticsHelper.RequestHaptics(HapticType.Low);
            
            new PopUpBuilder().SetTitleText("Don't quit Nooooo!!!")
                .SetContentText($"If you do you will.....KILL SCIENCE!")
                .SetPercentageHeight( 60 )
                .SetImage( $"Sprites/panicked_scientist")
                .SetConfirmAction(() =>
                {
                    HapticsHelper.RequestHaptics();
                    Logger.Log( $"Quitting the questionnaire" );
                    
                })
                .SetCancelAction(() =>
                {
                    HapticsHelper.RequestHaptics();
                    Logger.Log( $"Canceling the quit!!!" );
                })
                .AttachTo(_root).Build();
        }


        
        private void CreateFooter(VisualElement parent)
        {
            var footerContainer  = new ContainerBuilder().AddClass("questionnaire-footer").AttachTo(parent).Build();
            
            var submitButton = new ButtonBuilder().SetText("Check").AddClass("questionnaire-footer-button").OnClick(() =>
            {
                if (!QuestionnaireValidator.ValidateAnswers(_builtQuestionsList, _scrollview))
                {
                    HapticsHelper.RequestHaptics( HapticType.High );
                    Logger.LogWarning("answers incomplete - failed validation.");
                    return;
                }
                
                HapticsHelper.RequestHaptics( HapticType.Low );
                HandleSubmit();
                
            }).AttachTo(footerContainer).Build();
            
        }
        
        private void SetAnswer(int questionNumber, string answerText)
        {
            if (!_answerDataDictionary.TryGetValue(questionNumber, out var value))
            {
                Logger.LogError($"Attempted to set answer for invalid question number {questionNumber}");
                return;
            }
            
            value.AnswerText = answerText;

            if (value.IsAnswered) return;
            
            _progressBarController?.IncrementFill();
            value.IsAnswered = true;
        }

        private void HandleAnswer(int questionIndex, string answerText)
        {
            HapticsHelper.RequestHaptics(HapticType.Low);
            
            SetAnswer(questionIndex+1, answerText);
            
            int nextQuestionNumber = questionIndex + 1;
            
            if (nextQuestionNumber < _questionCount)
                ScrollViewHelper.JumpToElementSmooth( _scrollview, _builtQuestionsList[nextQuestionNumber].RootVisualElement, 0.3f,  300);

            _builtQuestionsList[questionIndex].ToggleWarningOutline(false);
        }
        
        private void HandleSubmit()
        {
            try
            {
                var questionnaireData = new QuestionnaireDataBuilder().SetTemplate(_questionnaireData)
                    .SetAnswers(_answerDataDictionary).Build();
                
                var jsonData = _jsonSerializer.Serialize(questionnaireData);
                
                Logger.Log( jsonData );

                var webData = new WebData
                {
                    Id = _questionnaireData.QuestionnaireID,
                    Data = jsonData
                };
                
                RequestDataUpload(webData);
                
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to handle submit: {e}");
            }
        }

      //  private void RequestHaptics(HapticType hapticType) => MessageBus.Instance.Broadcast( HapticsMessages.OnHapticsRequest, hapticType );
        
        private void RequestDataUpload( WebData webData ) =>  MessageBus.Instance.Broadcast( NeurekaDemoMessages.DataUploadRequestMessage, webData );
        
        
        //Helper function that is will be attached to the relevant button events
        //ensures the root visual element is cleared when the questionnaires life is ended
        private void Clear() => _root.Clear();
    }
}
