using System.Transactions;
using CodeBase.Documents.DemoA.Components;
using CodeBase.Helpers;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.RunTime;
using UnityEngine;
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

            for (int x = 0; x < 15; x++)
            {
                new MenuCardBuilder()
                    .SetParent(scrollview)
                    .SetTitle($"Menu Card {x+1}")
                    .SetProgress(Random.Range(0f, 1f))
                    .Build();
            }
              
           

           //var text = TextReader.ReadTextFile(PathToText);
            
            //Content Text
            //new LabelBuilder().SetText( text ).AddClass(UiStyleClassDefinitions.SharedContentText).AttachTo(scrollview).Build();
            
            //Build footer
            var footer = new ContainerBuilder().AddClass(UssClassNames.FooterContainer).AttachTo(PageRoot).Build();

            new ButtonBuilder().AddClass(UssClassNames.FooterButton).AttachTo(footer).Build();
            new ButtonBuilder().AddClass(UssClassNames.FooterButton).AttachTo(footer).Build();
            new ButtonBuilder().AddClass(UssClassNames.FooterButton).AttachTo(footer).Build();
            new ButtonBuilder().AddClass(UssClassNames.FooterButton).AttachTo(footer).Build();
            
            
            new FadeHelper(content, true, true);
        }

        private void BuildMenuCard(VisualElement parent)
        {
            var outerContainer = new ContainerBuilder().AddClass(UssClassNames.MenuCard).AttachTo(parent).Build();

            //left side
            var menuCardIconContainer = new ContainerBuilder().AddClass(UssClassNames.MenuCardIconContainer).SetBackgroundColor( ColorUtils.GetRandomColor() ).AttachTo(outerContainer).Build();

            var menuIcon = new ImageBuilder().AddClass(UssClassNames.MenuCardIcon).AttachTo(menuCardIconContainer).Build();
            
            //right side
            var menuTextContainer = new ContainerBuilder().AddClass(UssClassNames.MenuCardContentContainer).AttachTo(outerContainer).Build();

            var menuCardTitle = new LabelBuilder().SetText($"Menu Card Title").AddClass(UssClassNames.MenuCardTitle).AttachTo(menuTextContainer).Build();
            
            var menuCardContentText = new LabelBuilder().SetText($"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, ")
                .AddClass(UssClassNames.MenuCardBlurb).AttachTo(menuTextContainer).Build();

            var progressBarContainer = new ContainerBuilder().AddClass(UssClassNames.MenuCardProgressBarContainer).AttachTo(menuTextContainer).Build();
            
            BuildProgressBar( progressBarContainer );
        }

        private void BuildProgressBar(VisualElement parent)
        {
            new ProgressBarBuilder().SetWidthPercent(100).SetWidthPercent(25).SetFillClass(UssClassNames.MenuCardProgressBar).SetBackgroundColor(Color.green).SetMaxFill(1f).SetFillAmount( 1f  ).AttachTo(parent).Build();
        }
    }
    
    
    public static class ColorUtils
    {
        private static readonly System.Random random = new System.Random();

        public static Color GetRandomColor(float alpha = 1f)
        {
            return new Color(
                (float)random.NextDouble(),  // R
                (float)random.NextDouble(),  // G
                (float)random.NextDouble(),  // B
                alpha                        // A
            );
        }
    }
    
    
}
