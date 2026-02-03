using System;
using CodeBase.UiComponents.Page;
using CodeBase.UiComponents.Styles;
using FluentUI.Components;
using ToolBox.Services.Haptics;
using UiFrameWork.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Pages
{
    public class IntroPageBuilder
    {
        private readonly VisualElement _root;

        private Action _confirmQuit;
        private Action _cancelQuit;
        
        private string _title;
        private string _contentText;
        private string _imagePath;

        public IntroPageBuilder(VisualElement root)
        {
            _root = root;
        }
        
        public IntroPageBuilder SetTitle(string title)
        {
            _title = title;
            return this;
        }

        public IntroPageBuilder SetContentText(string contentText)
        {
            _contentText = contentText;
            return this;
        }

        public IntroPageBuilder SetImagePath(string imagePath)
        {
            _imagePath = imagePath;
            return this;
        }

        public IntroPageBuilder SetConfirmQuit(Action confirmQuit)
        {
            _confirmQuit = confirmQuit;
            return this;
        }

        public IntroPageBuilder SetCancelQuit(Action cancelQuit)
        {
            _cancelQuit = cancelQuit;
            
            return this;
        }
        
        public VisualElement Build()
        {
            var introPage = new ContainerBuilder().AddClass("overlay-root").AttachTo(_root).Build();
            
            CreateHeader(introPage);
            
            CreateContent(introPage);
            
            CreateFooter(introPage);
            
            return introPage;
        }
        
        private void CreateHeader(VisualElement parent)
        {
            var header = new StandardHeader.Builder()
                .SetParent(parent)
                .SetTitle(_title)
                .SetQuitButton(() => PopupFactory.CreateQuitPopup(parent,"Quitting Already!", "\nTime for a break? That's a good idea, you earned it.", _confirmQuit, _cancelQuit ))
                .SetHeaderStyle("header-nav")
                .SetTitleTextStyle("header-label")
                .SetButtonStyle("demo-header-button")
                .Build();
            
            header.SetBackButtonActive(false);
        }

        private void CreateContent(VisualElement parent)
        {
            //Build the content container
            var content = new ContainerBuilder().AddClass("centered-container").AttachTo(parent).Build();
            
            BuildContentImage(content);
            
            new LabelBuilder().SetText( _contentText ).AddClass("info-page-content-text").AttachTo(content).Build();
        }
        
        private void BuildContentImage(VisualElement parent)
        {
            new StandardImageBuilder().SetWidth(800).SetHeight(600).SetScaleMode(ScaleMode.ScaleToFit).SetResourcePath(_imagePath).AddClass("rounded-image").AttachTo(parent).Build();
        }

        private void CreateFooter(VisualElement parent)
        {
            var footerContainer  = new ContainerBuilder().AddClass("questionnaire-footer").AttachTo(parent).Build();
            
            new ButtonBuilder().SetText("Start").AddClass("questionnaire-footer-button").OnClick(() =>
            {
                HapticsHelper.RequestHaptics( HapticType.Low );
                parent.RemoveFromHierarchy();
                
            }).AttachTo(footerContainer).Build();
        }
    }
}