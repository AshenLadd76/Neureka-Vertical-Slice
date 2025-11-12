using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Pages
{
    public class SplashPage : BasePage
    {
        private const string BackgroundGradientPath = "Gradients/blue";
        private const string LobsterFontPath = "Fonts/Lobster/FontAsset/Lobster.asset";
        
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
            
            var gradientTexture = Resources.Load<Texture2D>(BackgroundGradientPath);

            if (gradientTexture == null)
            {
                Logger.Log("Splash Page Build Failed to load Gradient Texture");
                return;
            }

            var gradientImage = new ImageBuilder().SetTexture(gradientTexture).AttachTo(PageRoot).SetScaleMode(ScaleMode.StretchToFill).AddClass(UiStyleClassDefinitions.SplashGradient).Build();
            
            if (gradientImage == null)
            {
                Logger.Log("Splash Page Build Failed to load Gradient Image");
                return;
            }
            
            var overlayContainer = new ContainerBuilder().AddClass(UiStyleClassDefinitions.SplashOverlay).AttachTo(PageRoot).Build();


            
            
            var logo = new LabelBuilder()
                .SetTextColor(Color.white)
                .SetFontSize(120)
                .SetText("Travel")
                .AddClass( UiStyleClassDefinitions.SplashTitle )
                .AttachTo(overlayContainer)  // attach first
                .Build();
            
            var robotoFont = Resources.Load<Font>("Fonts/LCD/DS-DIGI");
            
            if (robotoFont == null)
            {
                Logger.LogError("Splash Page Build Failed to load Roboto Font");
                return;
            }
            
            var label = new LabelBuilder().SetText("Find Your Dream").AddClass(UiStyleClassDefinitions.SplashBlurb).AttachTo(overlayContainer).Build();
            
            var label2 = new LabelBuilder().SetText("Destination With Us").AddClass(UiStyleClassDefinitions.SplashBlurb).AttachTo(overlayContainer).Build();
            
            Logger.Log("Splash Page Build Success");
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
