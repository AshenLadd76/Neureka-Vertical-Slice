using System;

using CodeBase.UiComponents.Page;
using CodeBase.UiComponents.Styles;
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
            var headerNav = new ContainerBuilder().AddClass("header-nav").AttachTo(parent).Build();
            
            new ContainerBuilder().AddClass("header-spacer").AttachTo(headerNav).Build();
            
            new ButtonBuilder().SetText("X")
                .OnClick(() =>
                {
                    PopupFactory.CreateQuitPopup(_root, "Quitting huh ?!", "That makes sense. You have been working hard. Take a nice break and come back fresh!", _confirmQuit, _cancelQuit);
                })
                .AddClass("demo-header-button")
                .AddClass(UiStyleClassDefinitions.HeaderLabel)
                .AttachTo(headerNav)
                .Build();
            
            var buttonImage =  new ContainerBuilder().AddClass("button-image").AttachTo(headerNav).Build();
            
            var headerTitle =  new ContainerBuilder().AddClass("header-title").AttachTo(parent).Build();

            new LabelBuilder().SetText(_title).AddClass("header-label").AttachTo(headerTitle).Build();
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