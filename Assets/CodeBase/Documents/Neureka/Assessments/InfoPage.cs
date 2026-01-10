using System;
using System.Collections.Generic;
using CodeBase.Documents.DemoA;
using CodeBase.Documents.Neureka.Assessments.RiskFactors;
using CodeBase.UiComponents.Page;
using CodeBase.UiComponents.Pages;
using ToolBox.Extensions;
using ToolBox.Messaging;
using ToolBox.Services.Haptics;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Assessments
{
    public class InfoPage : BasePage
    {
        private int _blurbIndex;
        private int _blurbContentCount;
        
        private const string MainContainerStyle = "fullscreen-container";
        
        private readonly List<BlurbContent> _blurbContentList;
        private readonly InfoPageContent _infoPageContent;
        private readonly Action _onFinishedIntro;
        
        private StandardHeader _header;
        private VisualElement _contentContainer;
        private Image _currentContentImage;
        private Label _blurbLabel;
        private StandardFooter _footer;

        private string _footerButtonText;

        private VisualElement _parent;
        private VisualElement _pageRoot;
        
        
        
        private bool _hasScheduledFinalAction;
        
        private IDocument _document;

        
        public InfoPage(IDocument document, InfoPageContent content, VisualElement parent) : base(document)
        {
            PageIdentifier = PageID.InfoPage;
            
            _infoPageContent = content;

            _blurbContentList = _infoPageContent.ContentList;
            
            _onFinishedIntro = _infoPageContent.OnFinished;
            
            _document = document;
            _parent = parent;
            
        }
        
        protected override void Build()
        {
            CreatePage();
        }
        
        private void CreatePage()
        {
            Logger.Log( $"Creating Page {PageIdentifier}" );
            
            _pageRoot = new ContainerBuilder().AddClass(MainContainerStyle).AttachTo(_parent).Build();
            
            CreateHeader(_pageRoot);
            
            CreateContent(_pageRoot);
            
            CreateFooter(_pageRoot);
            
            _header.SetBackButtonActive(false);
            
            ToggleFooterButtons();
            
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
            
            UpdateImage();
            UpdateContentText();  
            ToggleFooterButtons();
        }
        
        private void CreateHeader(VisualElement parent)
        {
            _header = new StandardHeader.Builder()
                .SetParent(parent)
                .SetTitle(_infoPageContent.Title)
                .SetBackButton(Previous)
                .SetQuitButton(() => PopupFactory.CreateQuitPopup(_parent,"Quitting Already!", "\nThat's a good idea! \nTake a break and come back Fresh. You did good work.", ConfirmQuit, CancelQuit ))
                .SetHeaderStyle("header-nav")
                .SetTitleTextStyle("header-label")
                .SetButtonStyle("demo-header-button")
                .Build();
            
            _header.SetBackButtonActive(false);
        }
        
        private void CreateContent(VisualElement parent)
        {
            _contentContainer = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(parent).Build();
            
            BuildContentImage(_infoPageContent.ContentList[_blurbIndex].ImagePath);
            
            //Build the scrollView and add it to the content container
            var scrollView = BuildScrollView(_contentContainer);
            
            BuildContentText(scrollView);
            
            UpdateContentText();
        }
        
        private void BuildContentImage(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                Logger.LogError($"imagePath is empty");
                return;
            }
            
            _currentContentImage = new StandardImageBuilder().SetWidth(800).SetHeight(600).SetScaleMode(ScaleMode.ScaleToFit).SetResourcePath(imagePath).AddClass("rounded-image").AttachTo(_contentContainer).Build();
        }

        private void UpdateImage()
        {
            if (_currentContentImage == null)
            {
                Logger.LogError($"currentContentImage is null");
                return;
            }
            
            var imagePath = _infoPageContent.ContentList[_blurbIndex].ImagePath;

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                Logger.LogError($"imagePath is empty");
                return;
            }
            
            _currentContentImage.image = Resources.Load<Texture2D>(_blurbContentList[_blurbIndex].ImagePath);
        }
        
        private VisualElement BuildScrollView(VisualElement parent)
        {
            var scrollView = new ScrollViewBuilder().EnableInertia(true).SetPickingMode(PickingMode.Position)
                .AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(parent).Build();
            
            scrollView.contentContainer.style.flexDirection = FlexDirection.Column;
            scrollView.contentContainer.style.flexGrow = 1;
            
            return scrollView;
        }
        
        private void BuildContentText(VisualElement parent)
        {
            _blurbLabel = new LabelBuilder().AddClass("info-page-content-text").AttachTo(parent.contentContainer).Build();

            _blurbLabel.style.flexGrow = 1;
            _blurbLabel.style.flexShrink = 0;
            _blurbLabel.style.whiteSpace = WhiteSpace.Normal; // allow wrapping

        }
        
        private void UpdateContentText() => _blurbLabel.text = _blurbContentList[_blurbIndex].Text;

        
        private void CreateFooter(VisualElement parent)
        {
            _footer = new StandardFooter.Builder()
                .SetParent(parent)
                .SetPrimaryButton(Next, "Continue")
                .SetSecondaryButton(OnFinishedIntro, _infoPageContent.ButtonText)
                .SetFooterStyle("questionnaire-footer")
                .SetButtonStyle("questionnaire-footer-button")
                .Build();
        }

        private void ToggleFooterButtons()
        {
            bool isLastPage = _blurbIndex == _blurbContentList.Count - 1;
            
            _footer.SetPrimaryButtonActive(!isLastPage);
            _footer.SetSecondaryButtonActive(isLastPage);
        }

        private void ConfirmQuit()
        {
            HapticsHelper.RequestHaptics();
            MessageBus.Broadcast( nameof(DocumentServiceMessages.OnRequestOpenDocument), DocumentID.Nav);
            
            Close();
        }

        private void CancelQuit() => HapticsHelper.RequestHaptics();
        
        private void OnFinishedIntro()
        {
            _onFinishedIntro?.Invoke();
            Close();
        }

        public override void Close()
        {
            _document.Close();
            
            _parent.RemoveFromHierarchy();
            
            _pageRoot?.RemoveFromHierarchy();
            _pageRoot = null;
            
            base.Close();
            
          
            
        }
    }
}
