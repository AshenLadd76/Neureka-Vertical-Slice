using FluentUI.Components;
using UiFrameWork.Components;
using UiFrameWork.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.Documents.Neureka.Components
{
    public class ProgressBarController
    {
        private ProgressBarBuilder _progressBarBuilder;

        private readonly float _maxFillAmount;
        private readonly float _increment;
        
        private float _currentFillAmount;

        public ProgressBarController( string ussClass, float increment,  float maxFillAmount, VisualElement parent )
        {
            CreateProgressBar( ussClass, maxFillAmount, parent );
            
            _maxFillAmount = maxFillAmount;
            _increment = increment;
        }
        
        private void CreateProgressBar(string ussClass, float maxFillAmount, VisualElement parent )
        {
            var progressBarContainer = new ContainerBuilder().AddClass(ussClass).AttachTo(parent).Build();

            _progressBarBuilder = new ProgressBarBuilder();
            _progressBarBuilder.AttachTo(progressBarContainer);
            _progressBarBuilder.Build();

            _progressBarBuilder.SetMaxFill(maxFillAmount).SetFillAmount(0).OnMaxReached(OnComplete);
        }

        public void SetFillAmount(float fillAmount) => _progressBarBuilder.SetFillAmount(fillAmount);
        
        public void IncrementFill()
        {
            _currentFillAmount = Mathf.Min(_currentFillAmount + _increment, _maxFillAmount);

            SetFillAmount(_currentFillAmount);
        }

        public void DecrementFill()
        {
            _currentFillAmount = Mathf.Max(0, _currentFillAmount - _increment);
            SetFillAmount(_currentFillAmount);
        }

        private void OnComplete()
        {
            var fill = _progressBarBuilder.Fill;
            var root = _progressBarBuilder.Root;
            
            fill.ChangeColorAfterDelay( new Color32(255, 232, 29, 255), 1000 );
        }
        
    

    }
}
