using System;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Helpers
{
    public class FadeHelper
    {
        private const string BackgroundGradientPath = "Gradients/fade_2";

        
        private VisualElement _parent;
        
        private VisualElement _topFade;
        private VisualElement _bottomFade;
        
        private Texture2D _gradientTexture;

        private bool _enableTopFader;
        private bool _enableBottomFader;
        
        public FadeHelper(VisualElement parent, bool enableTopFader, bool enableBottomFader)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent), "Parent container cannot be null.");
            _enableTopFader = enableTopFader;
            _enableBottomFader = enableBottomFader;
            
            BuildFaders();
        }

        private void BuildFaders()
        {
            _gradientTexture = Resources.Load<Texture2D>(BackgroundGradientPath);
            
            if (_gradientTexture == null)
            {
                Logger.Log("Splash Page Build Failed to load Gradient Texture");
                return;
            }
            
            if (_enableTopFader)
            {
                _topFade = new ContainerBuilder().AddClass(UiStyleClassDefinitions.SharedScrollFadeTop).AttachTo(_parent).Build();
                new ImageBuilder().SetTexture(_gradientTexture).AttachTo(_topFade).FlipVertical().SetScaleMode(ScaleMode.StretchToFill).AddClass(UiStyleClassDefinitions.SplashGradient).Build();
            }


            if (_enableBottomFader)
            {
                _bottomFade = new ContainerBuilder().AddClass(UiStyleClassDefinitions.SharedScrollFadeBottom).AttachTo(_parent).Build();
                new ImageBuilder().SetTexture(_gradientTexture).AttachTo(_bottomFade).SetScaleMode(ScaleMode.StretchToFill).AddClass(UiStyleClassDefinitions.SplashGradient).Build();
            }
        }
    }
}
