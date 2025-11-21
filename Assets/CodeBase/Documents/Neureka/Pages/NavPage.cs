using System;
using System.Collections.Generic;
using CodeBase.Documents.DemoA;
using CodeBase.Documents.DemoA.Components;
using CodeBase.Documents.Neureka.Components;
using CodeBase.Helpers;
using CodeBase.Services;
using ToolBox.Messenger;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;
using Random = UnityEngine.Random;

namespace CodeBase.Documents.Neureka.Pages
{
    public class NavPage : BasePage
    {
        private readonly List<VisualElement> _allPages = new();
        
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
            
            base.Build();
            
            PageRoot = new ContainerBuilder().AddClass(UssClassNames.MainContainer).AttachTo(Root).Build();
            
            //Build header 
            var header = new ContainerBuilder().AddClass(UssClassNames.HeaderContainer).AttachTo(PageRoot).Build();

            var headerText = new LabelBuilder().SetText("Hi, Dean \n Welcome back!").AddClass(UssClassNames.HeaderLabel).AttachTo(header).Build();
            
            var headerImage = new ContainerBuilder().AddClass(UssClassNames.HeaderImage).AttachTo(header).Build();
            
            //Build content
            var content = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(PageRoot).Build();

            var scrollview = new ScrollViewBuilder().EnableInertia(true).AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(content).Build();

            List<Color> colorList = new List<Color>
            {
                new Color(0.172549f, 0.66f, 0.78f, 1f),
                new Color(0.8f, 0.19f, 0.45f, 1f),
                new Color(0.43f, 0.61f, 0.98f, 1f),
                new Color(0.6f, 0.61f, 0.98f, 1f),
                new Color(0.9f, 0.61f, 0.98f, 1f),
                new Color(0.2f, 0.3f, 0.5f, 1f),
            };
         
           //Questionnaires
           BuildNavSection( scrollview, "Depression", 1, colorList[0] );
           
           //Games
           BuildNavSection( scrollview, "Games", 23, colorList[1] );
           
           //Assessment
           BuildNavSection( scrollview, "Assessment", 1, colorList[3] );
           
           //Settings
           BuildNavSection(  scrollview, "Settings", 1, colorList[2] );
            
           BuildFooter();
           
           SelectNavPage(_allPages[0]);
           
            new FadeHelper(content, true, true);
        }

        private VisualElement BuildNavSection(ScrollView scrollView, string titlePrefix, int cardCount, Color color)
        {
            var container = new ContainerBuilder().AttachTo(scrollView).Build();

            for (int x = 0; x < cardCount; x++)
            {
                new MenuCardBuilder()
                    .SetParent(container)
                    .SetTitle($"{titlePrefix} {x+1}")
                    .SetProgress(Random.Range(0f, 1f))
                    .SetIconBackgroundColor( color )
                    .SetAction(MenuActions.RequestDocument("CESD-20"))
                    .Build();
            }
            
            container.style.display = DisplayStyle.None;
            _allPages.Add(container);

            return container;
        }

        private void BuildFooter()
        {
            //Build footer
            var footer = new ContainerBuilder().AddClass(UssClassNames.FooterContainer).AttachTo(PageRoot).Build();
            
            Logger.Log($"All pages count : {_allPages.Count}");

            for (int i = 0; i < _allPages.Count; i++)
            {
                int index = i;
                new ButtonBuilder().AddClass(UssClassNames.FooterButton).OnClick(() => { SelectNavPage(_allPages[index]); })
                    .AttachTo(footer).Build();
            }
        }
        
        private void SelectNavPage(VisualElement pageToShow)
        {
            foreach (var page in _allPages)
            {
                if (page == null) continue;
                
                page.style.display = page == pageToShow ? DisplayStyle.Flex : DisplayStyle.None;
            }
            
        }
    }
    
    
    public static class MenuActions
    {
        public static Action RequestDocument(string questionnaireId)
        {
            return () => 
            {
                Logger.Log( $"{questionnaireId}" );
                MessageBus.Instance.Broadcast(QuestionnaireService.OnRequestQuestionnaireMessage, questionnaireId);
            };
        }
    }
}
