using System;
using CodeBase.UiComponents.Factories;

using UiFrameWork.Components;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Components
{
    public class PopUpBuilder
    {
        // Matches USS --popup-close-duration
        private const long CloseAnimationDurationMs = 250;
        private const long BackgroundStartingInMs = 1;
        private const long SlideInDelay = 1;
        
        private string _blurbText;
        
        private VisualElement _root;

        private Action _confirmAction;
        private Action _cancelAction;

        private Texture2D _imageTexture2D;
        private Sprite _imageSprite;

        private string _imageResourcePath;
        private int _imageWidth;
        private int _imageHeight;

        private Length  _percentageHeight = new Length(60, LengthUnit.Percent);

        private string _contentText;
        
        private const string MissingText = "Missing Text";
        
        private VisualElement _popUpContainer;
        private VisualElement _background;
        
        private Button _confirmButton;
        private Button _cancelButton;


        public PopUpBuilder SetPercentageHeight(float height)
        {
            _percentageHeight = new Length(height, LengthUnit.Percent);
            return this;
        }
        
        public PopUpBuilder SetImage(Texture2D image)
        {
            _imageTexture2D = image;
            return this;
        }

        public PopUpBuilder SetImage(Sprite sprite)
        {
            _imageSprite = sprite;
            return this;
        }
        
        public PopUpBuilder SetImage(string resourcePath, int width = 0, int height = 0)
        {
            _imageResourcePath = resourcePath;
            _imageTexture2D = null;
            _imageSprite = null;

            if (width != 0 && height != 0)
            {
                _imageWidth = width;
                _imageHeight = height;
            }
            
            return this;
        }

        public PopUpBuilder SetTitleText(string blurbText)
        {
            _blurbText = blurbText;

            return this;
        }

        public PopUpBuilder SetContentText(string contentText)
        {
            _contentText = contentText;
            return this;
        }
        
        public PopUpBuilder SetConfirmAction(Action confirmAction)
        {
            _confirmAction = confirmAction;
            return this;
        }

        public PopUpBuilder SetCancelAction(Action cancelAction)
        {
            _cancelAction = cancelAction;
            return this;
        }
        
        public PopUpBuilder AttachTo(VisualElement parent)
        {
            _root = parent;
            return this;
        }
        
        public VisualElement Build()
        {
            _background = BuildBackgroundOverlay(_root);
            
            _popUpContainer =  new ContainerBuilder().AddClass(PopupBuilderUssClassNames.PopUpContainerStyle).AttachTo(_background).Build();
            
            if (_percentageHeight.value > 0)
                _popUpContainer.style.height = _percentageHeight;
            
            var popUpContent =  new ContainerBuilder().AddClass(PopupBuilderUssClassNames.PopUpContentStyle).AttachTo(_popUpContainer).Build();
            
            BuildImage(popUpContent);
           
            BuildTitle( popUpContent );
           
            BuildContentText(popUpContent);
            
            BuildFooter(popUpContent);
            
            AnimatePopUp();
            
            return _background;
        }

        private void AnimatePopUp()
        {
            _popUpContainer.schedule.Execute(_ =>
            {
                _popUpContainer.style.bottom = 0; // slide into view
            }).StartingIn(SlideInDelay);
        }
        
        private VisualElement BuildBackgroundOverlay(VisualElement parent)
        {
            const long startInMs = 5;
            
            var background = new ContainerBuilder().AddClass(PopupBuilderUssClassNames.BackgroundOverlayStyle).AttachTo(parent).Build();

            background.schedule.Execute(_ =>
            {
                background.AddToClassList(PopupBuilderUssClassNames.BackgroundActiveStyle);
            }).StartingIn(startInMs);
            
            return background;
        }

        private void BuildImage(VisualElement parent)
        {
            Texture2D texture = ResolveTexture();

            if (texture == null) return;
            
            // Use local variables for width/height, fallback to texture native size
            int width = _imageWidth != 0 ? _imageWidth : texture.width;
            int height = _imageHeight != 0 ? _imageHeight : texture.height;

            Logger.Log($"Displaying image {_imageResourcePath ?? "<passed texture/sprite>"} at {width}x{height}");

            new ImageBuilder()
                .SetTexture(texture)
                .SetWidth(width)
                .SetHeight(height)
                .AttachTo(parent)
                .Build();
        }
        
        private Texture2D ResolveTexture()
        {
            return _imageTexture2D 
                   ?? _imageSprite?.texture 
                   ?? (!string.IsNullOrEmpty(_imageResourcePath) ? Resources.Load<Texture2D>(_imageResourcePath) : null);
        }

        private void BuildTitle(VisualElement parent)
        {
            if (_blurbText == null) return;
            
            new ContainerBuilder().AddClass(PopupBuilderUssClassNames.PopUpSpacerStyle).AttachTo(parent).Build();
            
            var titleContainer = new ContainerBuilder().AttachTo(parent).AddClass( PopupBuilderUssClassNames.PopUpTitleStyle ).Build();

            _blurbText ??= MissingText;
            
            new LabelBuilder().SetText(_blurbText).AddClass(PopupBuilderUssClassNames.PopUpLabelStyle).AttachTo(parent).Build();
        }

        private void BuildContentText(VisualElement parent)
        {
            if (_contentText == null) return;
            
            var scrollview = new ScrollViewBuilder().EnableInertia(true).SetPickingMode(PickingMode.Position)
                .AddClass(PopupBuilderUssClassNames.PopUpContentStyle).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(parent).Build();
            
            var contentText = new LabelBuilder()
                .SetText(_contentText)
                .AttachTo(scrollview.contentContainer).AddClass("info-page-content-text").Build();
        }

        private void BuildFooter(VisualElement parent)
        {
            var popupFooter = new ContainerBuilder().AddClass(PopupBuilderUssClassNames.PopUpFooterStyle).AttachTo(parent).Build();

            if (_confirmAction != null)
            {
                _confirmButton = ButtonFactory.CreateButton(ButtonType.Confirm, "Confirm", () => { ExecuteAndClose(_confirmAction); }, popupFooter);
                _confirmButton.AddToClassList("questionnaire-footer-button-inverted");
            }

            if (_cancelAction != null)
            {
                _cancelButton = ButtonFactory.CreateButton(ButtonType.Cancel, "Cancel", () => { ExecuteAndClose(_cancelAction); }, popupFooter);

                _cancelButton.AddToClassList("questionnaire-footer-button-inverted");
            }
        }


        private void ExecuteAndClose(Action action)
        {
            Close(action);
        }

        private void Close(Action action = null)
        {
            if (_popUpContainer == null || _background == null)
                return;
            
            SlideOut(_popUpContainer);
                
            _background.schedule.Execute(_ =>
            {
                _background.AddToClassList(PopupBuilderUssClassNames.BackgroundInactiveStyle);
            }).StartingIn(BackgroundStartingInMs);

            _popUpContainer.schedule.Execute(_ =>
            {
                _popUpContainer?.RemoveFromHierarchy();
                _popUpContainer = null;
                
                _background?.RemoveFromHierarchy();
                _background = null;
                
                Reset();
                
                action?.Invoke();
                
            }).StartingIn(CloseAnimationDurationMs);
            
        }
        
        private void SlideOut(VisualElement popup) => popup.style.bottom = new Length(-60, LengthUnit.Percent);
        
        private void Reset()
        {
            _blurbText = null;
            _confirmAction = null;
            _cancelAction = null;
            _imageTexture2D = null;
            _imageSprite = null;
            _imageResourcePath = null;
            _imageWidth = 0;
            _imageHeight = 0;
            _root = null;
        }
    }

    public static class PopupBuilderUssClassNames
    {
        public const string BackgroundOverlayStyle = "fullscreen-fade-container";
        public const string BackgroundActiveStyle = "fullscreen-fade-container-active";
        public const string BackgroundInactiveStyle = "fullscreen-fade-container-inactive";
        
        public const string PopUpContainerStyle = "popup-container";
        public const string PopUpTitleStyle = "popup-title-container-centre";
        public const string PopUpContentStyle = "popup-content";
        public const string PopUpScrollViewStyle = "popup-scrollview";
        public const string PopUpContentTextStyle = "popup-content-text";
        public const string PopUpImageStyle = "popup-image";
        public const string PopUpLabelStyle = "popup-label";
        public const string PopUpFooterStyle = "popup-footer";
        public const string PopUpSpacerStyle = "popup-spacer";
    }
}
