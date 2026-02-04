using System;
using System.Collections.Generic;
using CodeBase.Documents.Neureka.Assessments.RiskFactors;
using CodeBase.UiComponents.Page;
using CodeBase.UiComponents.Pages;
using FluentUI.Components;
using ToolBox.Extensions;
using ToolBox.Messaging;
using ToolBox.Services.Haptics;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Assessments
{
    /// <summary>
    /// Represents a single informational page in the assessment flow.
    /// 
    /// Responsible for:
    /// - Displaying a sequence of text or blurbs with optional images
    /// - Handling navigation between blurb items (Next / Previous)
    /// - Managing header and footer UI elements
    /// - Invoking completion callbacks when the page is finished
    /// - Integrating haptic feedback on navigation actions
    /// </summary>
    
    public class InfoPage : BasePage
    {
        private int _blurbIndex;
        private int _blurbContentCount;
        
        private const string MainContainerStyle = "fullscreen-container";
        
        private readonly List<BlurbContent> _blurbContentList;
        private readonly InfoPageContent _infoPageContent;
        private readonly Action _onFinishedIntro;
        
        private readonly VisualElement _parent;
        private readonly IDocument _document;
        
        private VisualElement _pageRoot;
        private VisualElement _contentContainer;
        
        private StandardHeader _header;
        
        private Image _currentContentImage;
        private Label _blurbLabel;
        private StandardFooter _footer;
        
        private string _footerButtonText;
        
        private bool _hasScheduledFinalAction;

    
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoPage"/> class.
        /// </summary>
        /// <param name="document">The parent document that owns this page.</param>
        /// <param name="content">The content to display, including title, blurbs, images, and completion callback.</param>
        /// <param name="parent">The root VisualElement under which this page will be added.</param>
        public InfoPage(IDocument document, InfoPageContent content, VisualElement parent) : base(document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document), "IDocument cannot be null.");
            _infoPageContent = content ?? throw new ArgumentNullException(nameof(content), "InfoPageContent cannot be null.");
            _blurbContentList = _infoPageContent.ContentList ?? throw new ArgumentNullException(nameof(content.ContentList), "Content list cannot be null.");
            _onFinishedIntro = _infoPageContent.OnFinished;
            _parent = parent ?? throw new ArgumentNullException(nameof(parent), "Parent VisualElement cannot be null.");

            PageIdentifier = PageID.InfoPage;
            
        }
        
        /// <summary>
        /// Builds the InfoPage by creating its root, header, content, and footer elements.
        /// </summary>
        protected override void Build()
        {
            Logger.Log( $"Creating Page {PageIdentifier}" );
            
            _pageRoot = new ContainerBuilder().AddClass(MainContainerStyle).AttachTo(_parent).Build();
            
            CreateHeader(_pageRoot);
            
            CreateContent(_pageRoot);
            
            CreateFooter(_pageRoot);
            
            _header.SetBackButtonActive(false);
            
            ToggleFooterButtons();
        }
        // <summary>
        // Moves to the next blurb in the page content.
        // </summary>
        private void Next() => UpdateContent(_blurbIndex + 1);
        
        // <summary>
        // Moves to the previous blurb in the page content.
        // </summary>
        private void Previous() => UpdateContent(_blurbIndex - 1);
        
       
        // <summary>
        // Updates the displayed content (text and image) based on the specified blurb index.
        // Also updates header back button and footer buttons.
        // </summary>
        // <param name="newIndex">The index of the blurb to display.</param>
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
        
        // <summary>
        // Creates the standard page header, sets title, back, and quit buttons.
        // </summary>
        // <param name="parent">The parent VisualElement to attach the header to.</param>
        private void CreateHeader(VisualElement parent)
        {
            _header = new StandardHeader.Builder()
                .SetParent(parent)
                .SetTitle(_infoPageContent.Title)
                .SetBackButton(Previous)
                .SetQuitButton(() => PopupFactory.CreateQuitPopup(_parent,"Quitting Already!", "\nTime for a break? That's a good idea, you earned it.", ConfirmQuit, CancelQuit ))
                .SetHeaderStyle("header-nav")
                .SetTitleTextStyle("header-label")
                .SetButtonStyle("demo-header-button")
                .Build();
            
            _header.SetBackButtonActive(false);
        }
        
        
        // <summary>
        // Creates the main content container, including image and scrollable text.
        // </summary>
        // <param name="parent">The parent VisualElement to attach the content to.</param>
        private void CreateContent(VisualElement parent)
        {
            _contentContainer = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(parent).Build();
            
            BuildContentImage(_infoPageContent.ContentList[_blurbIndex].ImagePath);
            
            //Build the scrollView and add it to the content container
            var scrollView = BuildScrollView(_contentContainer);
            
            BuildContentText(scrollView);
            
            UpdateContentText();
        }
        
        // <summary>
        // Builds the content image from a given resource path and attaches it to the content container.
        // </summary>
        // <param name="imagePath">The path of the image inside the Resources folder.</param>
        private void BuildContentImage(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                Logger.LogError($"imagePath is empty");
                return;
            }
            
            _currentContentImage = new StandardImageBuilder().SetWidth(800).SetHeight(600).SetScaleMode(ScaleMode.ScaleToFit).SetResourcePath(imagePath).AddClass("rounded-image").AttachTo(_contentContainer).Build();
        }

        // <summary>
        // Updates the content image based on the currently active blurb.
        // </summary>
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
        
        // <summary>
        // Builds the scroll view container for the page content.
        // </summary>
        // <param name="parent">The parent VisualElement to attach the scroll view to.</param>
        // <returns>The constructed ScrollView element.</returns>
        private VisualElement BuildScrollView(VisualElement parent)
        {
            var scrollView = new ScrollViewBuilder().EnableInertia(true).SetPickingMode(PickingMode.Position)
                .AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(parent).Build();
            
            scrollView.contentContainer.style.flexDirection = FlexDirection.Column;
            scrollView.contentContainer.style.flexGrow = 1;
            
            return scrollView;
        }
        
        // <summary>
        // Builds the label used for the blurb text inside the scroll view.
        // </summary>
        // <param name="parent">The parent VisualElement (scroll view content container) to attach the label to.</param>
        private void BuildContentText(VisualElement parent)
        {
            _blurbLabel = new LabelBuilder().AddClass("info-page-content-text").AttachTo(parent.contentContainer).Build();

            _blurbLabel.style.flexGrow = 1;
            _blurbLabel.style.flexShrink = 0;
            _blurbLabel.style.whiteSpace = WhiteSpace.Normal; // allow wrapping
        }
        
        // <summary>
        // Updates the label text to match the currently active blurb.
        // </summary>
        private void UpdateContentText() => _blurbLabel.text = _blurbContentList[_blurbIndex].Text;

        
        // <summary>
        // Creates the footer with primary and secondary buttons.
        // Primary is used for navigation, secondary for page completion.
        // </summary>
        // <param name="parent">The parent VisualElement to attach the footer to.</param>
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

        // <summary>
        // Updates the visibility of primary and secondary footer buttons based on the current blurb index.
        // </summary>
        private void ToggleFooterButtons()
        {
            bool isLastPage = _blurbIndex == _blurbContentList.Count - 1;
            
            _footer.SetPrimaryButtonActive(!isLastPage);
            _footer.SetSecondaryButtonActive(isLastPage);
        }

        // <summary>
        // Handles confirmation of quitting the page.
        // Closes the page and returns to the navigation screen.
        // </summary>
        private void ConfirmQuit()
        {
            HapticsHelper.RequestHaptics();
            MessageBus.Broadcast( nameof(DocumentServiceMessages.OnRequestOpenDocument), DocumentID.Nav);
            
            Close();
        }

        // <summary>
        // Handles cancellation of quitting the page, providing haptic feedback.
        // </summary>
        private void CancelQuit() => HapticsHelper.RequestHaptics();
        
        // <summary>
        // Invoked when the user finishes reading all blurbs on the page.
        // Calls the provided completion callback and closes the page.
        // </summary>
        private void OnFinishedIntro()
        {
            _onFinishedIntro?.Invoke();
            Close();
        }

        /// <summary>
        /// Closes the page, removes it from the hierarchy, and notifies the parent document.
        /// </summary>
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
