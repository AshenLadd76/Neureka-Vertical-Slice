using CodeBase.Documents.DemoA.Components;
using CodeBase.Helpers;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.DemoA.Pages
{
    public class InfoPage : BasePage
    {
        private const string BackgroundGradientPath = "Gradients/fade";
        
        public InfoPage()
        {
            PageIdentifier = PageID.InfoPage;
        }
        
        protected override void Build()
        {
            base.Build();
            
            PageRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.SplashRoot).AttachTo(Root).Build();
            
            //Content
            var content =  new ContainerBuilder().AddClass(UiStyleClassDefinitions.SharedContent).AttachTo(PageRoot).Build();
            
            
            //Main image container 
            var shadowContainer = new ContainerBuilder().AddClass(UiStyleClassDefinitions.ImageShadowContainer).AttachTo(content).Build();
            
            var imageContainer = new ContainerBuilder().AddClass(UiStyleClassDefinitions.ImageContainer).AttachTo(shadowContainer).Build();
            
            var imageHeader = new ContainerBuilder().AddClass(UiStyleClassDefinitions.InfoImageHeader).AttachTo(imageContainer).Build();
            
            new ButtonBuilder().AddClass(UiStyleClassDefinitions.ImageHeaderButton).AttachTo(imageHeader).Build();
            
            new ButtonBuilder().AddClass(UiStyleClassDefinitions.ImageHeaderButton).AttachTo(imageHeader).Build();
            
            new ContainerBuilder().AddClass(UiStyleClassDefinitions.InfoImageBlurb).AttachTo(imageContainer).Build();
            
            //Main image ends
            
            
            
            new ContainerBuilder().AddClass(UiStyleClassDefinitions.InfoOverview).AttachTo(content).Build();
            
            var scrollview = new ScrollViewBuilder().AddClass(UiStyleClassDefinitions.SharedScrollViewNoScrollBars).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(content).Build();
            
            var text = FileReader.ReadTextFile($"Assets/TestText/SampleText.txt");
            
            new LabelBuilder().SetText( text ).AddClass(UiStyleClassDefinitions.SharedContentText).AttachTo(scrollview).Build();
            
            var footer =  new ContainerBuilder().AddClass(UiStyleClassDefinitions.SharedFooter).AttachTo(PageRoot).Build();

            new PrimaryButton( footer, $"Welcome",()=> { Logger.Log($"Primary button clicked.");});
            
            new FadeHelper(content, false, true);

        }
    }
}
