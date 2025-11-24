using UiFrameWork.Components;
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
            
            _progressBarBuilder = new ProgressBarBuilder().SetMaxFill(maxFillAmount).SetFillAmount(0)
                .AttachTo(progressBarContainer);
          
            
            
            _progressBarBuilder.Build();
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
    }
}
