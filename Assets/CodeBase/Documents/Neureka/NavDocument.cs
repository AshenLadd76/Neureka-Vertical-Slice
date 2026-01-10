using System;
using System.Collections.Generic;
using CodeBase.Documents.DemoA;
using CodeBase.Documents.Neureka.Components;
using CodeBase.Helpers;
using ToolBox.Extensions;
using ToolBox.Messaging;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;
using Random = UnityEngine.Random;

namespace CodeBase.Documents.Neureka
{
    public class NavDocument : BaseDocument
    {
        private readonly List<SectionData> _sectionDataList = new();
        
        private readonly List<VisualElement> _allPages = new();

        private VisualElement _navRoot;
        private VisualElement _content;
        private ScrollView _scrollView;

        public override bool ShouldCache => true;

        public override void Build(VisualElement root)
        {
            Logger.Log("Building Nav Document");
            
            if (root == null)
            {
                Logger.Log("Splash Page Build Failed");
                return;
            }
            
            base.Build(root);
            
            LoadSectionDataList();
            
            _navRoot = new ContainerBuilder().AddClass(UssClassNames.MainContainer).AttachTo(DocumentRoot).Build();
            
            //Build content
            _content = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(_navRoot).Build();
            
            _scrollView = new ScrollViewBuilder().EnableInertia(true).AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(_content).Build();

            var topSpacer = new ContainerBuilder().AddClass("scrollview-top-spacer").AttachTo(_scrollView).Build();
           
            LoadNavSections(_scrollView); 
           
            BuildFooter();
           
            SelectNavPage(_allPages[0]);
           
            new FadeHelper(_content, true, true);
        }

        public override void Open(VisualElement root)
        {
            base.Open(root);
            
            Logger.Log($"Opening Nav Document { _allPages.Count }");
           
            
            _navRoot.style.display = DisplayStyle.Flex;
            _content.style.display = DisplayStyle.Flex;
            _scrollView.style.display = DisplayStyle.Flex;
            
            ShowRootAndActivePage();
        }

        public override void Close()
        {
            base.Close();
            _navRoot.style.display = DisplayStyle.None;
        }

        private void LoadNavSections(ScrollView scrollView)
        {
            if (_sectionDataList.IsNullOrEmpty())
            {
                Logger.Log("Loading Nav Sections Failed");
                return;
            }
            
            _allPages.Clear();
            
            foreach (var t in _sectionDataList)
                BuildNavSection(scrollView, t);
        }
        
        private VisualElement BuildNavSection(ScrollView scrollView, SectionData sectionData)
        {
            var container = new ContainerBuilder().AddClass("scroll-view-content").AttachTo(scrollView).Build();

            for (int x = 0; x < sectionData.CardCount; x++)
            {
                new MenuCardBuilder()
                    .SetParent(container)
                    .SetTitle($"{sectionData.Title}")
                    .SetProgress(Random.Range(0f, 1f))
                    .SetIconBackgroundColor( sectionData.Color )
                    .SetAction(MenuActions.RequestDocument(sectionData.DcoumentID))
                    .Build();
            }
            
            container.style.display = DisplayStyle.Flex;
            
            _allPages.Add(container);

            return container;
        }
        private void BuildFooter()
        {
            //Build footer
            var footer = new ContainerBuilder().AddClass(UssClassNames.FooterContainer).AttachTo(_navRoot).Build();
            
            Logger.Log($"All pages count : {_allPages.Count}");
            
            for (int i = 0; i < _allPages.Count; i++)
            {
                int index = i;
                
                var footerButton = new ButtonBuilder().AddClass(UssClassNames.FooterButton).OnClick(() =>
                {
                    if (_allPages.IsNullOrEmpty()) return;
                    
                    SelectNavPage(_allPages[index]);
                        
                }).AttachTo(footer).Build();
            }
        }


        private VisualElement _lastSelectedPage;
        private void SelectNavPage(VisualElement pageToShow)
        {
            if (pageToShow == null && !_allPages.IsNullOrEmpty())
                pageToShow = _allPages[0];
            
            _lastSelectedPage = pageToShow;
            
            foreach (var page in _allPages)
            {
                if (page == null)
                {
                    Logger.Log("SelectNavPage Failed");
                    continue;
                }

                page.style.display = page == pageToShow ? DisplayStyle.Flex : DisplayStyle.None;
            }
            
        }
        
        
        private void LoadSectionDataList()
        {
            _sectionDataList.Clear();
            
            _sectionDataList.Add( new SectionData( "RiskFactors", 1,   new Color(0.43f, 0.61f, 0.98f, 1f), nameof(DocumentID.RiskFactors)));
            _sectionDataList.Add( new SectionData( "Games", 23, new Color(0.172549f, 0.66f, 0.78f, 1f), "CESD-20"));
            _sectionDataList.Add( new SectionData( "Assessment", 1,   new Color(0.43f, 0.61f, 0.98f, 1f), "CESD-20" ));
            _sectionDataList.Add(new SectionData("Settings", 1, new Color(0.6f, 0.61f, 0.98f, 1f), "CESD-20")); 
        }

        public virtual void ShowRootAndActivePage()
        {
            if (_navRoot == null) return;

            
            _navRoot.style.display = DisplayStyle.Flex;

            // Make the currently selected page visible, if any
            foreach (var page in _allPages)
            {
                if (page == null) continue;

                // Show only the last selected page, or the first page
                page.style.display = page == _lastSelectedPage ? DisplayStyle.Flex : DisplayStyle.None;
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
                MessageBus.Broadcast( nameof(DocumentServiceMessages.OnRequestOpenDocument), DocumentID.RiskFactors );
            };
        }
    }


    public class SectionData
    {
        public string Title { get; set; }
        public int CardCount { get; set; }
        public Color Color { get; set; }
        
        public string DcoumentID { get; set; }

        // Constructor with validation
        public SectionData(string title, int cardCount, Color color, string dcoumentID)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));

            if (cardCount < 0)
                throw new ArgumentOutOfRangeException(nameof(cardCount), "CardCount cannot be negative.");

            // Color is a struct, it can't be null, but you can add checks if needed
            // For example, you might not want fully transparent colors:
            if (color.a < 0f || color.a > 1f)
                throw new ArgumentOutOfRangeException(nameof(color), "Color alpha must be between 0 and 1.");

            Title = title;
            CardCount = cardCount;
            Color = color;
            DcoumentID = dcoumentID;
        }
        
    }
}
