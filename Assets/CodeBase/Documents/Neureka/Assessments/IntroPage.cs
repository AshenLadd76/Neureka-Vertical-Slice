using System;
using System.Collections.Generic;
using CodeBase.Documents.DemoA;
using CodeBase.UiComponents.Footers;
using CodeBase.UiComponents.Styles;
using ToolBox.Extensions;
using ToolBox.Services.Haptics;
using ToolBox.Utils;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UnityEngine.UIElements;

namespace CodeBase.Documents.Neureka.Assessments
{
    public class IntroPage : BasePage
    {
        private int _blurbIndex = -1;
        private int _blurbContentCount;
        
        private const string MainContainerStyle = "fullscreen-container";
        
        private List<BlurbContent> _blurbContentList;
        
        private IDocument _parentDocument;
        
        private IntroPageContent _introPageContent;
        
        private Label _blurbLabel;
        
        
        public IntroPage(IDocument document, IntroPageContent content) : base(document)
        {
            _parentDocument = document;
            PageIdentifier = PageID.InfoPage;
            _introPageContent = content;

            _blurbContentList = _introPageContent.ContentList;
        }


        protected override void Build()
        {
            base.Build();
            
            CreatePage();
        }
        
        private void CreatePage()
        {
            var pageRoot = new ContainerBuilder().AddClass(MainContainerStyle).AttachTo(Root).Build();
            
            CreateHeader(pageRoot);
            
            CreateContent(pageRoot);
            
            CreateFooter(pageRoot);
            
            UpdateContent(0);
        }

        
        private void Next()
        {
            if (_blurbContentList.IsNullOrEmpty()) return;

            UpdateContent(_blurbIndex + 1);
        }

        private void Previous()
        {
            if (_blurbContentList.IsNullOrEmpty()) return;

            UpdateContent(_blurbIndex - 1);
        }
        

        private void UpdateContent(int newIndex)
        {
            if (_blurbContentList == null || _blurbContentList.Count == 0) return;

            // Clamp between 0 and last index
            _blurbIndex = Math.Clamp(newIndex, 0, _blurbContentList.Count - 1);

            Logger.Log($"Blurb {_blurbIndex}: { _blurbContentList[_blurbIndex].Blurb }");

            _blurbLabel.text = _blurbContentList[_blurbIndex].Blurb;
        }
        
        private void CreateHeader(VisualElement parent)
        {
            var headerNav = new ContainerBuilder().AddClass("header-nav").AttachTo(parent).Build();

            new ButtonBuilder().SetText("<")
                .OnClick(Previous)
                .AddClass("demo-header-button")
                .AddClass(UiStyleClassDefinitions.HeaderLabel)
                .AttachTo(headerNav)
                .Build();
            
            new ButtonBuilder().SetText("X")
                .OnClick(() => { Logger.Log( "X" ); })
                .AddClass("demo-header-button")
                .AddClass(UiStyleClassDefinitions.HeaderLabel)
                .AttachTo(headerNav)
                .Build();
            
            var headerTitle =  new ContainerBuilder().AddClass("header-title").AttachTo(parent).Build();

            var label = new LabelBuilder().SetText(_introPageContent.Title).AddClass("header-label").AttachTo(headerTitle).Build();
        }
        
        private void CreateContent(VisualElement parent)
        {
            var content = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(parent).Build();

            //Build the scrollview and add it to the content container
            var scrollview = new ScrollViewBuilder().EnableInertia(true).SetPickingMode(PickingMode.Position)
                .AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(content).Build();
            
            scrollview.contentContainer.style.flexDirection = FlexDirection.Column;
            scrollview.contentContainer.style.flexGrow = 1;
            
              
            _blurbLabel = new LabelBuilder().AddClass(UiStyleClassDefinitions.SharedContentText).AttachTo(scrollview.contentContainer).Build();
            _blurbLabel.style.flexShrink = 0;
            _blurbLabel.style.whiteSpace = WhiteSpace.Normal; // allow wrapping
        }

        private void CreateFooter(VisualElement parent)
        {
            var footer = new StandardFooter.Builder()
                .SetParent(parent)
                .SetPrimaryButton(Next, "Continue")
                .SetFooterStyle("questionnaire-footer")
                .SetButtonStyle("questionnaire-footer-button")
                .Build();
        }
    }
}
