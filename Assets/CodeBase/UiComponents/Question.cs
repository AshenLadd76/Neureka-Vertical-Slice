using System;
using CodeBase.Documents.DemoA;
using CodeBase.UiComponents.Headers;
using FluentUI.Components;
using ToolBox.Extensions;
using ToolBox.Utils;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents
{
    
    /// <summary>
    /// Builder class for creating a question UI element with single or multiple choice options.
    /// Supports attaching to a parent container, customizing CSS classes, and handling option selection.
    /// </summary>
    
    public class Question 
    {
        private  int _questionIndex;
        private string _questionText;
        
        private string[] _answers;

        private  Action<int, string> _onOptionSelected;

        private VisualElement _parent;

        private string _labelClass;
        private string _containerClass;
        
        private const string DefaultContainerClass = "default-container-class";
        private const string DefaultLabelClass = "default-label-class";
        private const string ValidationOutline = "unanswered-highlight";
        

        private bool _isMultiSelection;

        public bool IsAnswered { get; private set; }
        public VisualElement RootVisualElement { get; private set; }
        
        /// <summary>
        /// Sets whether the question allows multiple selections.
        /// </summary>
        /// <param name="b">True for multi-selection, false for single selection.</param>
        /// <returns>The current Question builder instance.</returns>
        public Question SetMultiSelection(bool b)
        {
            _isMultiSelection = b;
            return this;
        }
        
        /// <summary>
        /// Sets the index of the question.
        /// </summary>
        /// <param name="index">Zero-based index of the question.</param>
        /// <returns>The current Question builder instance.</returns>
        public Question SetIndex(int index)
        {
            _questionIndex = index;
            return this;
        }

        /// <summary>
        /// Sets the main question text.
        /// </summary>
        /// <param name="text">The text of the question.</param>
        /// <returns>The current Question builder instance.</returns>
        public Question SetQuestionText(string text)
        {
            _questionText = text;
            return this;
        }
        
        
        /// <summary>
        /// Sets the answer options for the question.
        /// </summary>
        /// <param name="answers">Array of answer strings.</param>
        /// <returns>The current Question builder instance.</returns>
        public Question SetAnswers(string[] answers)
        {
            _answers = answers;
            return this;
        }
        
        /// <summary>
        /// Sets the callback to invoke when an option is selected.
        /// </summary>
        /// <param name="onOptionSelected">Callback taking question index and selected answer text.</param>
        /// <returns>The current Question builder instance.</returns>
        public Question SetOnOptionSelected(Action<int, string> onOptionSelected)
        {
            _onOptionSelected = onOptionSelected;
            return this;
        }

        
        /// <summary>
        /// Adds a CSS class to the question label.
        /// </summary>
        /// <param name="className">CSS class name for the label.</param>
        /// <returns>The current Question builder instance.</returns>
        public Question AddLabelClass(string className)
        {
            _labelClass = className;
            return this;
        }

        /// <summary>
        /// Adds a CSS class to the question container.
        /// </summary>
        /// <param name="className">CSS class name for the container.</param>
        /// <returns>The current Question builder instance.</returns>
        public Question AddContainerClass(string className)
        {
            _containerClass = className;
            return this;
        }

        
        /// <summary>
        /// Attaches the question UI to a parent VisualElement.
        /// </summary>
        /// <param name="visualElement">The parent VisualElement to attach to.</param>
        /// <returns>The current Question builder instance.</returns>
        public Question AttachTo(VisualElement visualElement)
        {
            _parent = visualElement;
            return this;
        }

        public void ToggleWarningOutline(bool b)
        {
            if( b )
                RootVisualElement.AddToClassList( ValidationOutline );
            else
                RootVisualElement.RemoveFromClassList( ValidationOutline );
            
        }
        
        

        /// <summary>
        /// Builds the question UI element hierarchy and attaches it to the parent.
        /// </summary>
        /// <returns>The root VisualElement containing the question and its options.</returns>
        /// <exception cref="InvalidOperationException">Thrown if required fields (text, answers, parent) are missing.</exception>

        public Question Build()
        {
            if( string.IsNullOrEmpty(_questionText) )
                throw new InvalidOperationException("Question text is empty");
            
            if( _answers.IsNullOrEmpty() )
                throw new InvalidOperationException( "Answers must be set before building." );
            
            //if( _parent == null )
                //throw new InvalidOperationException( "Parent must be set before building." );
            
            
            var outerContainer = new ContainerBuilder()
                .AddClass(ClassCheck(_containerClass, DefaultContainerClass))
                .Build();

            _parent?.Add(outerContainer);

            new LabelBuilder().SetText($"{_questionIndex+1}. {_questionText}").AddClass(ClassCheck(_labelClass, DefaultLabelClass)).AttachTo(outerContainer).Build();

           var optionsContainer = new ContainerBuilder().AddClass(ClassCheck(_containerClass, DefaultContainerClass)).AttachTo(outerContainer).Build();

           var choiceGroupBuilder = new ChoiceGroupBuilder().AttachTo(optionsContainer).AllowMultipleSelection(_isMultiSelection);
           
            for (int x = 0; x < _answers.Length; x++)
            {
                string answerText = _answers[x];
                
                choiceGroupBuilder.AddOption(answerText, (i, b) => { _onOptionSelected?.Invoke(_questionIndex, answerText ); IsAnswered = true; });
            }

            choiceGroupBuilder.Build();


            RootVisualElement = outerContainer;
            
            //Add our response here
            return this;
        }

        private string ClassCheck(string className, string defaultClass) => string.IsNullOrEmpty(className) ? defaultClass : className;
    }
}
