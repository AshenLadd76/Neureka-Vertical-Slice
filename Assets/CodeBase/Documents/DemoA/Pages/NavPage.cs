using CodeBase.Helpers;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.RunTime;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.DemoA.Pages
{
    public class NavPage : BasePage
    {
        private const string headerSpritePath = "";
        private const string PathToText = "Assets/TestText/SampleText.txt";
        
        
        public NavPage(IDocument document) : base(document)
        {
            PageIdentifier = PageID.NavPage;
        }

        protected override void Build()
        {
            
            
            if (Root == null)
            {
                Logger.Log("Splash Page Build Failed");
                return;
            }
            
            Logger.Log($"Building Splash Page {Root.name}");
            
            base.Build();
            
            PageRoot = new ContainerBuilder().AddClass(UssClassNames.MainContainer).AttachTo(Root).Build();
            
            //Build header 
            var header = new ContainerBuilder().AddClass(UssClassNames.HeaderContainer).AttachTo(PageRoot).Build();

            var headerText = new LabelBuilder().SetText("Hi, Dean \n Welcome back!").AddClass(UssClassNames.HeaderLabel).AttachTo(header).Build();
            
            var headerImage = new ContainerBuilder().AddClass(UssClassNames.HeaderImage).AttachTo(header).Build();
            
            //Build content
            var content = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(PageRoot).Build();

            var scrollview = new ScrollViewBuilder().AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(content).Build();

            var text = TextReader.ReadTextFile(PathToText);
            
            //Content Text
            new LabelBuilder().SetText( text ).AddClass(UiStyleClassDefinitions.SharedContentText).AttachTo(scrollview).Build();
            
            //Build footer
            var footer = new ContainerBuilder().AddClass(UssClassNames.FooterContainer).AttachTo(PageRoot).Build();
            
            new FadeHelper(content, true, true);
        }
    }
}
