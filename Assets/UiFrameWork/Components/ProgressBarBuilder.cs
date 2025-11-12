using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class ProgressBarBuilder : BaseBuilder<VisualElement, ProgressBarBuilder>
    {
        private float _maxFill;
        private float _currentFill;
        
        private VisualElement _fillElement;
        private string _fillClassName;

        private Action _onMinReached, _onMaxReached;
        
        private Label _label;
        
        public ProgressBarBuilder SetMaxFill(float value)
        {
            _maxFill = MathF.Max(0.00001f, value);
            return this;
        }
        
        public ProgressBarBuilder SetFillClass(string className)
        {
            _fillClassName  = className;

            CreateFillElement();
            
            return this;
        }
        
        private ProgressBarBuilder CreateFillElement()
        {
            _fillElement = new VisualElement();
            _fillElement.AddToClassList(_fillClassName);
            VisualElement.Add(_fillElement);
            
            _label = VisualElement.Q<Label>();
            
            return this;
        }
        
        public ProgressBarBuilder SetFillAmount(float fillAmount)
        {
            _currentFill = Mathf.Clamp(fillAmount, 0, _maxFill);
            
            Debug.Log( $"Setting fill amount of {_maxFill} to {_currentFill} " );
            
            _fillElement.style.width = _currentFill;
            
            if( _currentFill <= 0 ) _onMinReached?.Invoke();
            
            if( Mathf.Approximately(_currentFill, _maxFill) ) _onMaxReached?.Invoke();
            
            return this;
        }

        public ProgressBarBuilder OnMinReached(Action callback)
        {
            _onMinReached = callback ?? throw new ArgumentNullException(nameof(callback));

            return this;

        }

        public ProgressBarBuilder OnMaxReached(Action callback)
        {
            _onMaxReached = callback ?? throw new ArgumentNullException(nameof(callback));
            return this;
        }

        public ProgressBarBuilder SetLabelText(string text)
        {
            _label ??= VisualElement.Q<Label>();
            
            _label.style.unityTextAlign = TextAnchor.MiddleCenter;

            if (_label != null)
                _label.text = text;

            return this;
        }

        public ProgressBarBuilder ToggleLabel(bool value)
        {
            _label ??= VisualElement.Q<Label>();

            _label.visible = value;
            return this;
        }
        
    }
}
