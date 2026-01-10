using System;
using System.Collections.Generic;
using CodeBase.Documents.DemoA;
using CodeBase.Documents.Neureka.Components;
using CodeBase.Documents.Neureka.Navigation;
using CodeBase.Helpers;

using ToolBox.Extensions;

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

        private readonly List<SectionData> _sectionDataList = new();
        
        public NavPage(IDocument document) : base(document)
        {
            PageIdentifier = PageID.NavPage;
        }

        protected override void Build()
        {
          
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


        private List<Button> _footerButtonList = new();
        private void BuildFooter()
        {
            //Build footer
            var footer = new ContainerBuilder().AddClass(UssClassNames.FooterContainer).AttachTo(PageRoot).Build();
            
            Logger.Log($"All pages count : {_allPages.Count}");

        

            for (int i = 0; i < _allPages.Count; i++)
            {
                int index = i;
                
                var footerButton = new ButtonBuilder().AddClass(UssClassNames.FooterButton).OnClick(() =>
                {
                    if (_allPages.IsNullOrEmpty()) return;

                    Logger.Log($"Clicked on page {index}");
                    SelectNavPage(_allPages[index]);
                        
                }).AttachTo(footer).Build();
            }
        }
        
        private void SelectNavPage(VisualElement pageToShow)
        {
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
    }
    
    
    
}
