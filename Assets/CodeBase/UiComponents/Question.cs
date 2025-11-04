using System;
using CodeBase.Documents.DemoA;
using CodeBase.UiComponents.Headers;
using ToolBox.Utils;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents
{
    public class Question
    {
        private readonly int _questionIndex;
        private readonly string[] _answers;

        private readonly Action<int, string> _onOptionSelected;

        private VisualElement _parent;
        
        public VisualElement RootElement { get; private set; }
        
        public Question(int questionIndex, string question, string[] answers, VisualElement parent, Action<int, string> onOptionSelected = null)
        {
            
            _questionIndex = questionIndex;
            _answers = answers;
            _onOptionSelected = onOptionSelected;
            _parent = parent;
            
            RootElement = Build(question);
        }
        

        private VisualElement Build(string question)
        {
            var outerContainer = new ContainerBuilder()
                .AddClass("question-container")
                .AttachTo(_parent)
                .Build();   
            
           new LabelBuilder().SetText($"{_questionIndex+1}. {question}").AddClass("question-container-label").AttachTo(outerContainer).Build();

           var optionsContainer = new ContainerBuilder().AddClass("question-container").AttachTo(outerContainer).Build();

           var choiceGroupBuilder = new ChoiceGroupBuilder().AttachTo(optionsContainer).AllowMultipleSelection(false);
           
            for (int x = 0; x < _answers.Length; x++)
            {
                int answerIndex = x;
                string answerText = _answers[answerIndex];
                
                choiceGroupBuilder.AddOption(_answers[x], (i, b) => { _onOptionSelected?.Invoke(_questionIndex, answerText ); });
            }

            choiceGroupBuilder.Build();
            
            
            //Add our response here
            return outerContainer;
        }
    }
}
