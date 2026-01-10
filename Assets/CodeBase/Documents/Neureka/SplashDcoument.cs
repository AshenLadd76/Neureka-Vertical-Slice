using CodeBase.UiComponents.Styles;
using ToolBox.Helpers;
using ToolBox.Messaging;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka
{
    public class SplashDocument : BaseDocument
    {
        private const string BackgroundGradientPath = "Gradients/blue";
        private const string LogoTexturePath = "Sprites/Neureka/logo_neureka";
    
        private ICoroutineRunner _coroutineRunner;
        private Coroutine _openPageCoroutine;
        private VisualElement _pageRoot;
        
        public SplashDocument(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public override void Build(VisualElement root)
        {
            base.Build(root);
            
            _pageRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.SplashRoot).AttachTo(DocumentRoot).OnClick(()=> { OnClickAction(true); }).Build();
            
            SetBackGroundGradientTexture(_pageRoot);
            
            var overlayContainer = new ContainerBuilder().AddClass(UiStyleClassDefinitions.SplashOverlay).AttachTo(_pageRoot).Build();

            var logoTexture  = Resources.Load<Texture2D>(LogoTexturePath);

            if (logoTexture == null)
            {
                Logger.Log("Failed to load Logo texture");
                return;
            }
            
            var logoImage = new ImageBuilder().SetTexture(logoTexture).AddClass("logo").AttachTo(overlayContainer).Build();
            
            Logger.Log("Splash Page Build Success");
            
        }
        
        private void SetBackGroundGradientTexture(VisualElement parent)
        {
            var gradientTexture = Resources.Load<Texture2D>(BackgroundGradientPath);

            if (gradientTexture == null)
            {
                Logger.Log("Splash Page Build Failed to load Gradient Texture");
                return;
            }

            new ImageBuilder().SetTexture(gradientTexture).AttachTo(parent).SetScaleMode(ScaleMode.StretchToFill).AddClass(UiStyleClassDefinitions.SplashGradient).Build();
        }

        private void OnClickAction(bool closeSelf)
        {
           Logger.Log($"OnClickAction splash document {closeSelf}");
           MessageBus.Broadcast(nameof(DocumentServiceMessages.OnRequestOpenDocument), DocumentID.Nav);
           
           _pageRoot.RemoveFromHierarchy();
           Close();

        }
    }
}
