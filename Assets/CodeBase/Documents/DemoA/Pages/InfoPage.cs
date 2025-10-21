using CodeBase.Documents.DemoA.Components;
using CodeBase.Helpers;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.RunTime;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.DemoA.Pages
{
    public class InfoPage : BasePage
    {
        private const string PathToText = "Assets/TestText/SampleText.txt";
        
        public InfoPage(IDocument document) : base(document)
        {
            PageIdentifier = PageID.InfoPage;
        }
        
        protected override void Build()
        {
            base.Build();
            
            PageRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.SplashRoot).AttachTo(Root).Build();
            
            //Content
            var content =  new ContainerBuilder().AddClass(UiStyleClassDefinitions.SharedContent).AttachTo(PageRoot).Build();
            
            //Add image card to content
            new ImageCard( content, ()=>{BackAction(true);}, () => { Logger.Log($"right header button clicked"); });
            
            new ContainerBuilder().AddClass(UiStyleClassDefinitions.InfoOverview).AttachTo(content).Build();
            
            //ScrollView
            var scrollview = new ScrollViewBuilder().AddClass(UiStyleClassDefinitions.SharedScrollViewNoScrollBars).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(content).Build();
            
            var text = TextReader.ReadTextFile(PathToText);
            
            //Content Text
            new LabelBuilder().SetText( text ).AddClass(UiStyleClassDefinitions.SharedContentText).AttachTo(scrollview).Build();
            
            //Footer
            new PageFooter(PageRoot, "Welcome", () => { Logger.Log("Welcome to CodeBase.DemoA.Pages"); });
            
            new FadeHelper(content, false, true);
        }

        private void BackAction(bool closeSelf)
        {
            
            ParentDocument.OpenPage(PageID.Splash);
            
            if (closeSelf)
                ParentDocument.ClosePage(PageIdentifier, PageRoot);
        }
    }
}
