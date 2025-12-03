using System;
using System.Collections.Generic;
using CodeBase.Documents.DemoA;
using CodeBase.Documents.Neureka.Components;
using CodeBase.UiComponents.Footers;
using CodeBase.UiComponents.Headers;
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
        
        private IntroPageContent _introPageContent;
        
        private Label _blurbLabel;

        private VisualElement _pageRoot;
        
        public IntroPage(IDocument document, IntroPageContent content) : base(document)
        {
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
            _pageRoot = new ContainerBuilder().AddClass(MainContainerStyle).AttachTo(Root).Build();
            
            CreateHeader(_pageRoot);
            
            CreateContent(_pageRoot);
            
            CreateFooter(_pageRoot);
            
            UpdateContent(0);
        }

        
        private void Next() => UpdateContent(_blurbIndex + 1);
        
        private void Previous() => UpdateContent(_blurbIndex - 1);
        
        
        private void UpdateContent(int newIndex)
        {
            if (_blurbContentList.IsNullOrEmpty()) return;
            
            HapticsHelper.RequestHaptics(HapticType.Low);

            // Clamp between 0 and last index
            _blurbIndex = Math.Clamp(newIndex, 0, _blurbContentList.Count - 1);

            _header.SetBackButtonActive(_blurbIndex != 0);
            
            _blurbLabel.text = _blurbContentList[_blurbIndex].Blurb;
        }


        private StandardHeader _header;
        private void CreateHeader(VisualElement parent)
        {
            _header = new StandardHeader.Builder()
                .SetParent(parent)
                .SetBackButton(Previous)
                .SetQuitButton(CreateQuitPopUp)
                .SetHeaderStyle("header-nav")
                .SetTitleTextStyle("header-label")
                .SetButtonStyle("demo-header-button")
                .Build();
            
            _header.SetBackButtonActive(false);
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
               // .SetSecondaryButton(Previous, "Cancel")
                .SetFooterStyle("questionnaire-footer")
                .SetButtonStyle("questionnaire-footer-button")
                .Build();
        }
        
        
        private VisualElement _popup;
        private void CreateQuitPopUp()
        {
            HapticsHelper.RequestHaptics(HapticType.Low);
            
            _popup = new PopUpBuilder().SetTitleText("Don't quit Nooooo!!!")
                .SetContentText($"If you do you will.....KILL SCIENCE!")
                .SetPercentageHeight( 60 )
                .SetImage( $"Sprites/panicked_scientist")
                .SetConfirmAction(() =>
                {
                    HapticsHelper.RequestHaptics();
                    Logger.Log( $"Quitting the questionnaire" );
                    Close();
                    
                })
                .SetCancelAction(() =>
                {
                    HapticsHelper.RequestHaptics();
                    Logger.Log( $"Canceling the quit!!!" );
                })
                .AttachTo(Root).Build();
        }

        public override void Close()
        {
            _pageRoot?.RemoveFromHierarchy();
            _pageRoot = null;

            _popup?.RemoveFromHierarchy();
            _popup = null;
            
            base.Close();
        }
    }
}
