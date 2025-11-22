using UiFrameWork.Components;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Components
{
    public class ProgressBarController
    {
        private ProgressBarBuilder _progressBarBuilder;

        public ProgressBarController( string ussClass, float maxFillAmount, VisualElement parent )
        {
            CreateProgressBar( ussClass, maxFillAmount, parent );
        }
        
        private void CreateProgressBar(string ussClass, float maxFillAmount, VisualElement parent )
        {
            var progressBarContainer = new ContainerBuilder().AddClass(ussClass).AttachTo(parent).Build();
            
            _progressBarBuilder = new ProgressBarBuilder().SetMaxFill(maxFillAmount).SetFillAmount(0)
                .AttachTo(progressBarContainer);
          
            
            
            _progressBarBuilder.Build();
        }

        public void SetFillAmount(float fillAmount) => _progressBarBuilder.SetFillAmount(fillAmount);
        
        public void SetFillAmountAimatated( float filAmount ) => _progressBarBuilder.SetFillAmount(filAmount);
    }
}
