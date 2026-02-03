using CodeBase.UiComponents.Styles;
using FluentUI.Components;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Pages
{
    public class SplashPage : BasePage
    {
        private const string BackgroundGradientPath = "Gradients/blue";
        private const string LogoTexturePath = "Sprites/Neureka/logo_neureka";
       
        
        public SplashPage(IDocument document) : base(document)
        {
            PageIdentifier = PageID.Splash;
        }
        
        protected override void Build()
        {
            base.Build();
            
            if (Root == null)
            {
                Logger.Log("Splash Page Build Failed");
                return;
            }
            
            Logger.Log($"Building Splash Page {Root.name}");
            
            PageRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.SplashRoot).AttachTo(Root).OnClick(()=> { OnClickAction(true); }).Build();
            
            SetBackGroundGradientTexture(PageRoot);
            
            var overlayContainer = new ContainerBuilder().AddClass(UiStyleClassDefinitions.SplashOverlay).AttachTo(PageRoot).Build();

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
            ParentDocument.OpenPage(PageID.NavPage);

            if (closeSelf)
            {
                ParentDocument.ClosePage(PageIdentifier, PageRoot);
                //Close();
            }
        }
        
    }
}
