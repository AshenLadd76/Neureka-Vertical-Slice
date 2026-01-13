using System;
using System.Collections.Generic;
using CodeBase.Documents.Neureka;
using CodeBase.Documents.Neureka.Components;
using CodeBase.Questionnaires;
using CodeBase.UiComponents.Styles;
using ToolBox.Helpers;
using ToolBox.Messaging;
using ToolBox.Services.Haptics;
using UiFrameWork.Components;
using UiFrameWork.Helpers;
using UiFrameWork.RunTime;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.UiComponents.Pages
{
    public class QuestionnairePageBuilder
    {
        private const string ImagePath = "Assessments/RiskFactors/Images/";
        private const string MainContainerStyle = "fullscreen-container";
        private readonly int _questionCount;
        private int _questionsAnsweredCount;
        
        private ScrollView _scrollview;
        
        private VisualElement _root;

        private readonly List<Question> _builtQuestionsList = new();
        
        private readonly Dictionary<int, AnswerData> _answerDataDictionary = new();

        private readonly StandardQuestionnaireTemplate _questionnaireData;
        
        private readonly ISerializer _jsonSerializer;
        
        private ProgressBarController _progressBarController;

        private VisualElement _documentRoot;

        private readonly Action _onFinished;
        
        private QuestionnaireSubmissionHandler _questionnaireSubmissionHandler;
        
        private Button _submitButton;

        private VisualElement _introPage;
        
        public QuestionnairePageBuilder(StandardQuestionnaireTemplate questionnaireData, VisualElement root, ISerializer jsonSerializer, IDocument parentDocument = null, Action onFinished = null)
        {
            if (questionnaireData == null)
                throw new ArgumentNullException(nameof(questionnaireData), "Questionnaire data cannot be null.");

            if (questionnaireData.Questions == null || questionnaireData.Questions.Length == 0)
                throw new ArgumentException("Questionnaire must contain at least one question.", nameof(questionnaireData));

            _root = root ?? throw new ArgumentNullException(nameof(root), "Parent VisualElement cannot be null.");

            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer), "JSON serializer cannot be null.");

            _questionCount = questionnaireData.Questions.Length;
            
            _questionnaireData = questionnaireData;
            
            _onFinished = onFinished;
            
            InitializeAnswerDictionary(questionnaireData);
        }

        private void InitializeAnswerDictionary(StandardQuestionnaireTemplate questionnaireData)
        {
            for (var x = 0; x < _questionCount; x++)
            {
                var questionNumber = x + 1;
                var questionText = questionnaireData.Questions[x];
               
                _answerDataDictionary.Add(questionNumber, new AnswerData(questionNumber, questionText, "" ));
            }
        }
        
        public void Build()
        {
            CreateQuestionnaire(_root);
            CreateIntroPage(_root);
        }
        
        private void CreateIntroPage(VisualElement root)
        {
            _introPage =  new IntroPageBuilder(root)
                .SetTitle(_questionnaireData.QuestionnaireName)
                .SetContentText(_questionnaireData.QuestionnaireIntroduction)
                .SetImagePath(ImagePath+_questionnaireData.QuestionnaireIntroductionImage)
                .SetConfirmQuit(ConfirmQuit)
                .SetCancelQuit(CancelQuit)
                .Build();
        }

        private void CreateQuestionnaire(VisualElement root)
        {
            _documentRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.DocumentRoot).AttachTo(root).Build();
            
            //Build the container
            var pageRoot = new ContainerBuilder().AddClass(MainContainerStyle).AttachTo(_documentRoot).Build();
            
            CreateHeader(pageRoot);
            
            CreateProgressBar(pageRoot);
            
            //Build the content container
            var content = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(pageRoot).Build();
            
            //Build the scrollview and add it to the content container
            _scrollview = new ScrollViewBuilder().EnableInertia(true).SetPickingMode(PickingMode.Position).AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).Build();
            
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
            
            new ContainerBuilder().AddClass("header-spacer").AttachTo(headerNav).Build();
            
            new ButtonBuilder().SetText("X")
                .OnClick(() => { PopupFactory.CreateQuitPopup(_root, "Quitting !!!", "If you do you will.....KILL SCIENCE!", ConfirmQuit, CancelQuit); })
                .AddClass("demo-header-button")
                .AddClass(UiStyleClassDefinitions.HeaderLabel)
                .AttachTo(headerNav)
                .Build();
            
            var headerTitle =  new ContainerBuilder().AddClass("header-title").AttachTo(parent).Build();

            new LabelBuilder().SetText(_questionnaireData.QuestionnaireName).AddClass("header-label").AttachTo(headerTitle).Build();
        }
        
        private void ConfirmQuit()
        {
            HapticsHelper.RequestHaptics();
  
            MessageBus.Broadcast( DocumentServiceMessages.OnRequestOpenDocument.ToString(), DocumentID.Nav );
            
            Close();
        }

        private void CancelQuit() => HapticsHelper.RequestHaptics();
           
        
        private void ConfirmFinished()
        {
            HapticsHelper.RequestHaptics(); 
            
            _onFinished?.Invoke();
            
            Close();
        }

        private void CreateProgressBar(VisualElement parent)
        {
            _progressBarController = new ProgressBarController("progress-bar-header", 1f,_questionCount,parent);
            
            _progressBarController.SetFillAmount(0);
        }
        

        private void CreateFooter(VisualElement parent)
        {
            var questionnaireSubmissionHandler =
                new QuestionnaireSubmissionHandler(_questionnaireData, _answerDataDictionary, _jsonSerializer,
                    () => { PopupFactory.CreateConfirmationPopup(_root , "Thank You!", "For taking the time to complete this questionnaire", ConfirmFinished);  });
            
            var footerContainer  = new ContainerBuilder().AddClass("questionnaire-footer").AttachTo(parent).Build();
            
            _submitButton = new ButtonBuilder().SetText("Check").AddClass("questionnaire-footer-button").OnClick(() =>
            {
                if (!QuestionnaireValidator.ValidateAnswers(_builtQuestionsList, _scrollview))
                {
                    HapticsHelper.RequestHaptics( HapticType.High );
                    Logger.LogWarning("answers incomplete - failed validation.");
                    return;
                }
                
                HapticsHelper.RequestHaptics( HapticType.Low );
                
                questionnaireSubmissionHandler.Submit();
                
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

            _questionsAnsweredCount++;
            
            value.IsAnswered = true;

            if (_questionsAnsweredCount >= _questionCount)
            {
                Logger.Log( $"All questions have been answered" );
                
                _submitButton.text = "Submit";
            }
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
        
        
      //Ensure all questionnaire visual elements are removed and GC collected
        private void Close()
        {
            _introPage.RemoveFromHierarchy();
            
            _documentRoot?.RemoveFromHierarchy();
            _documentRoot = null;
        }
    }
}
