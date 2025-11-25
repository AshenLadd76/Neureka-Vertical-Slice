using System;
using CodeBase.UiComponents.Factories;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Components
{
    public class PopUpBuilder
    {
        private const string BackgroundOverlayStyle = "fullscreen-fade-container";
        private const string BackgroundActiveStyle = "fullscreen-fade-container-active";
        private const string BackgroundInactiveStyle = "fullscreen-fade-container-inactive";
        
        private const string PopUpContainerStyle = "popup-container";
        private const string PopUpContentStyle = "popup-content";
        private const string PopUpImageStyle = "popup-image";
        private const string PopUpLabelStyle = "popup-label";
        private const string PopUpFooterStyle = "popup-footer";

        private string _blurbText;

        private VisualElement _popUpRoot;
        
        
        private VisualElement _root;

        private Action _confirmAction;
        private Action _cancelAction;

        private Texture2D _imageTexture2D;
        private Sprite _imageSprite;

        private string _imageResourcePath;
        private int _imageWidth;
        private int _imageHeight;
        
        
        public PopUpBuilder()
        {
            
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

        public PopUpBuilder SetBlurbText(string blurbText)
        {
            _blurbText = blurbText;

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
           
            
            var background = BuildBackgroundOverlay(_root);
            
            var popUpContainer =  new ContainerBuilder().AddClass(PopUpContainerStyle).AttachTo(_root).Build();
            var popUpContent =  new ContainerBuilder().AddClass(PopUpContentStyle).AttachTo(popUpContainer).Build();
           
            //var popupImage = new ContainerBuilder().AddClass(PopUpImageStyle).AttachTo(popUpContent).Build();

           // var popUpImage = new ImageBuilder().SetTexture(_imageTexture2D).Build();
           
           BuildImage(popUpContent);
           
            var popupTitle = new LabelBuilder().SetText(_blurbText).AddClass(PopUpLabelStyle).AttachTo(popUpContent).Build();
            
            var popupFooter = new ContainerBuilder().AddClass(PopUpFooterStyle).AttachTo(popUpContent).Build();
            
            ButtonFactory.CreateButton(ButtonType.Confirm, "Confirm", _confirmAction , popupFooter).AddToClassList( DemoHubUssDefinitions.MenuButton );
            
            ButtonFactory.CreateButton(ButtonType.Cancel, "Cancel", () =>
            {
                _cancelAction?.Invoke();
                Close(popUpContainer,background);

            }, popupFooter).AddToClassList( DemoHubUssDefinitions.MenuButton );
            
            _root.MarkDirtyRepaint();
            popUpContainer.schedule.Execute(_ =>
            {
                popUpContainer.style.bottom = 0; // slide into view
            }).StartingIn(1);


            return popUpContainer;
        }

        private VisualElement BuildBackgroundOverlay(VisualElement parent)
        {
            const long startInMs = 1;
            
            var background = new ContainerBuilder().AddClass(BackgroundOverlayStyle).AttachTo(parent).Build();

            background.schedule.Execute(_ =>
            {
                background.AddToClassList(BackgroundActiveStyle);
            }).StartingIn(startInMs);
            
            return background;
        }

        private void BuildImage(VisualElement parent)
        {
            Texture2D texture = _imageTexture2D;

            // Prioritize passed-in texture, then sprite (converted to texture), then resource
            if (texture == null && _imageSprite != null)
            {
                texture = _imageSprite.texture;
            }

            if (texture == null && !string.IsNullOrEmpty(_imageResourcePath))
            {
                Logger.Log($"Loading image {_imageResourcePath}");
                texture = Resources.Load<Texture2D>(_imageResourcePath);
            }

            if (texture == null)
            {
                // No image to display
                return;
            }

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


        private void Close(VisualElement popUpContainer, VisualElement background)
        {
            const int bottomPosition = -60;
            const long backgroundStartingInMs = 1;
            const long popUpContainerStartingInMs = 500;
            
            popUpContainer.style.bottom = new Length(bottomPosition, LengthUnit.Percent);
                
            background.schedule.Execute(_ =>
            {
                background.AddToClassList(BackgroundInactiveStyle);
            }).StartingIn(backgroundStartingInMs);

            popUpContainer.schedule.Execute(_ =>
            {
                _root.Remove(background);
                _root.Remove(popUpContainer);
                
                Reset();
                
            }).StartingIn(popUpContainerStartingInMs);
        }
        
        private void Reset()
        {
            _blurbText = null;
            _confirmAction = null;
            _cancelAction = null;
            _root = null;
        }
    }
}
