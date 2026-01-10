using System.Collections.Generic;
using CodeBase.Documents.DemoA;
using CodeBase.Documents.Neureka.Components;
using CodeBase.Helpers;
using ToolBox.Extensions;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;
using Random = UnityEngine.Random;

namespace CodeBase.Documents.Neureka.Navigation
{
    public class NavDocument : BaseDocument
    {
        private readonly List<SectionData> _sectionDataList = new();
        
        private readonly List<VisualElement> _allPages = new();

        private const string NavIconResourcePath = "Navigation/NavIconsSo";

        private VisualElement _navRoot;
        private VisualElement _content;
        private ScrollView _scrollView;
        
        private List<Image> _imageList = new();
        
        private Color unSelectedColor = new Color(.56f, .74f, .89f, 1f);  
        private Color selectedColor = new Color(1f, 1f, 1f, 1f);
        
       

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
            
            var header = new ContainerBuilder().AddClass(NavUssClassNames.NavHeader).AttachTo(_navRoot).Build();
            
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
            
            Logger.Log( $"Loading nav icons" );
            var iconList = LoadNavIcons();

            if (iconList.IsNullOrEmpty())
            {
                Logger.LogError("Loading nav icons failed ...icon list is empty");
            }
            
            
            //Build footer
            var footer = new ContainerBuilder().AddClass(NavUssClassNames.NaVFooter).AttachTo(_navRoot).Build();
            
            Logger.Log($"All pages count : {_allPages.Count}");
            
            _imageList.Clear();
            
            for (int i = 0; i < _allPages.Count; i++)
            {
                int index = i;
                
                var footerButton = new ContainerBuilder().AddClass(NavUssClassNames.NavFooterButton).OnClick(() =>
                {
                    if (_allPages.IsNullOrEmpty()) return;
                    
                    SelectIconImage(index);
                    SelectNavPage(_allPages[index]);
                        
                }).AttachTo(footer).Build();


                if (i < iconList.Count)
                {
                    Logger.Log( $"Adding Icon: {iconList[i].Name}" );
                    var iconImage = new ImageBuilder().SetSprite(iconList[i].Selected).AddClass(NavUssClassNames.NavFooterIcon).AttachTo(footerButton).Build();
                    
                    iconImage.tintColor = unSelectedColor;
                    
                    _imageList.Add(iconImage);
                }
                else
                    Logger.LogWarning($"No nav icon assigned for page index {i}");
            }
            
            SelectIconImage(0);
        }

        private List<NavIcon> LoadNavIcons()
        {
            var navIconsSO = Resources.Load<NavigationIconsSo>(NavIconResourcePath);

            if (navIconsSO != null && !navIconsSO.NavIconList.IsNullOrEmpty()) return navIconsSO.NavIconList;
            
            Debug.LogError("Failed to load NavIcons SO!");
            
            return new List<NavIcon>();

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

        private void SelectIconImage(int index)
        {
            if (!_imageList.IsIndexValid(index)) return;
            
            foreach (var image in _imageList)
                image.tintColor = unSelectedColor;
            
            _imageList[index].tintColor = selectedColor;
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


    public static class NavUssClassNames
    {
        public const string NavHeader = "nav-header-container";
        public const string NaVFooter = "nav-footer-container";
        public const string NavFooterButton = "nav-footer-button";
        public const string NavFooterIcon = "nav-footer-icon";
       
    }
}
