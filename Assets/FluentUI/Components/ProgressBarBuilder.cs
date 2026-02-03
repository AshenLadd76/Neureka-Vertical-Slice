using System;
using UiFrameWork.Builders;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace UiFrameWork.Components
{
    public class ProgressBarBuilder : BaseBuilder<VisualElement, ProgressBarBuilder>
    {
        private const string DefaultFillClass = "progress-bar-fill";
        private const string DefaultRootClass = "progress-bar-new";
        private const string DefaultTrackClass = "progress-bar-track";
        
        private string _fillClass = DefaultFillClass;
        private string _trackClass = DefaultTrackClass;
        private string _rootClass = DefaultRootClass;
        
        private float _maxFill;
        private float _currentFill;

        public VisualElement FillElement { get; set; }
        
        // UI elements
        private readonly VisualElement _root;

        public VisualElement Root => _root;

        private readonly VisualElement _track;
        private readonly VisualElement _fill;

        public VisualElement Fill => _fill;

        private  VisualElement _label;
        
        //Call backs
        private Action _onMinReached;
        private Action _onMaxReached;

        private bool _minFired;
        private bool _maxFired;



        public ProgressBarBuilder()
        {
            // Root element (always exists)
            VisualElement.AddToClassList(_rootClass);
            _root = VisualElement; // BaseBuilder.VisualElement is now the root
            
            
            // Track element (background)
            _track = new ContainerBuilder()
                .AddClass(_trackClass)
                .AttachTo(_root)
                .Build();

            // Fill element
            _fill = new ContainerBuilder()
                .AddClass(_fillClass)
                .AttachTo(_track)
                .Build();

            // initialize fill width and height
            _fill.style.width = Length.Percent(0);
            _fill.style.height = Length.Percent(100);
        }


        public ProgressBarBuilder SetMaxFill(float maxFill)
        {
            float minValue = 0.00001f;
            
            _maxFill = Mathf.Max(minValue, maxFill);
            
            return this;
        }
        
   
        
        /// <summary>
        /// Replace the fill CSS class (removes previous one).
        /// </summary>
        public ProgressBarBuilder SetFillClass(string className)
        {
            if (string.IsNullOrWhiteSpace(className)) return this;

            _fill.RemoveFromClassList(_fillClass);
            _fillClass = className;
            _fill.AddToClassList(_fillClass);
            return this;
        }
        
        /// <summary>
        /// Replace the track CSS class (removes previous one).
        /// </summary>
        public ProgressBarBuilder SetTrackClass(string className)
        {
            if (string.IsNullOrWhiteSpace(className)) return this;

            _track.RemoveFromClassList(_trackClass);
            _trackClass = className;
            _track.AddToClassList(_trackClass);
            return this;
        }
        
        /// <summary>
        /// Set root class to allow theming the whole control.
        /// </summary>
        public ProgressBarBuilder SetRootClass(string className)
        {
            if (string.IsNullOrWhiteSpace(className)) return this;

            _root.RemoveFromClassList(_rootClass);
            _rootClass = className;
            _root.AddToClassList(_rootClass);
            return this;
        }
        
        /// <summary>
        /// Set explicit pixel size of the control.
        /// </summary>
        public ProgressBarBuilder SetSize(float width, float height)
        {
            if (width > 0) _root.style.width = width;
            if (height > 0) _root.style.height = height;
            return this;
        }
        
        /// <summary>
        /// Set the fill amount using the same units as MaxFill (absolute). Clamped to 0..MaxFill.
        /// This updates the visual fill using percent width.
        /// </summary>
        public ProgressBarBuilder SetFillAmount(float absoluteValue)
        {
            var prev = _currentFill;
            _currentFill = Mathf.Clamp(absoluteValue, 0f, _maxFill);

            // compute percent (0..100)
            float percent = (_maxFill <= 0) ? 0f : (_currentFill / _maxFill) * 100f;
            _fill.style.width = Length.Percent(percent);

            HandleThresholds(prev, _currentFill);

            return this;
        }
        
        /// <summary>
        /// Set the fill using a normalized 0..1 value. Convenience wrapper.
        /// </summary>
        public ProgressBarBuilder SetFillNormalized(float normalized)
        {
            normalized = Mathf.Clamp01(normalized);
            return SetFillAmount(normalized * _maxFill);
        }

        /// <summary>
        /// Get current normalized value (0..1).
        /// </summary>
        public float GetFillNormalized() => (_maxFill <= 0f) ? 0f : (_currentFill / _maxFill);
        
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
        
        private void HandleThresholds(float prevValue, float newValue)
        {
            // min threshold
            if (newValue <= 0f)
            {
                if (!_minFired)
                {
                    _onMinReached?.Invoke();
                    _minFired = true;
                }
            }
            else
            {
                _minFired = false;
            }

            // max threshold (use approx to tolerate floats)
            if (Mathf.Approximately(newValue, _maxFill) || newValue >= _maxFill)
            {
                if (!_maxFired)
                {
                    _onMaxReached?.Invoke();
                    _maxFired = true;
                }
            }
            else
            {
                _maxFired = false;
            }
        }
        
        
        public ProgressBarBuilder SetLabelText(string text)
        {
            _label = new LabelBuilder()
                .AddClass("progress-bar-label")
                .SetTextAlignment(TextAnchor.MiddleCenter)
                .SetPosition(Position.Absolute)
                .SetText(text)
                .SetVisibility(Visibility.Visible)
                .AttachTo(_root).Build();
        
            return this;
        }
        
    }
}
