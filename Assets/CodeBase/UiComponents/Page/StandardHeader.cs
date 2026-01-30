using System;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Page
{
    public class StandardHeader
    {
        private Action _backAction;
        private Action _quitAction;
        
        private string _titleText = "Standard Header";
      
        private VisualElement _parent;
       
        private string _headerStyle = "header-nav";
        private string _buttonStyle = "demo-header-button";
        
        private readonly string _headerTitleContainerStyle = "header-title";
        
        private string _titleTextStyle = "header-label";

        public VisualElement Root { get; private set; }

        private Button _backButton { get; set;  }
        private Button _quitButton { get; set;  }
        
        public void SetBackButtonActive(bool active)
        {
            _backButton.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void SetQuitButtonActive(bool active)
        {
            _quitButton.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void BuildHeader()
        {
            var headerNav = new ContainerBuilder().AddClass(_headerStyle).AttachTo(_parent).Build();

            _backButton = new ButtonBuilder()
                .OnClick(() =>
                {
                    _backAction?.Invoke();
                })
                .AddClass(_buttonStyle)
                .AddClass(UiStyleClassDefinitions.HeaderLabel)
                .AttachTo(headerNav)
                .Build();
            
            new ContainerBuilder().AttachTo(headerNav).AddClass("header-spacer").Build();
            
           _quitButton = new ButtonBuilder()
                .OnClick(() =>
                {
                    _quitAction.Invoke();
                    
                })
                .AddClass(_buttonStyle)
                .AddClass(UiStyleClassDefinitions.HeaderLabel)
                .AttachTo(headerNav)
                .Build();
           
            LoadButtonImage(_quitButton, "Sprites/quit", 48,48);
            
            LoadButtonImage(_backButton, "Sprites/back", 36, 48);
         
            var headerTitle =  new ContainerBuilder().AddClass(_headerTitleContainerStyle).AttachTo(_parent).Build();
            
            var label = new LabelBuilder().SetText(_titleText).AddClass(_titleTextStyle).AttachTo(headerTitle).Build();
        }


        private void LoadButtonImage(Button button, string imagePath, int width, int height)
        {
            Texture2D backgroundTexture = Resources.Load<Texture2D>(imagePath);

            button.style.backgroundImage = new StyleBackground(backgroundTexture);

            button.style.backgroundSize = new BackgroundSize( new Length(width, LengthUnit.Pixel), new Length(height, LengthUnit.Pixel) );


        }
        
        // Builder nested class
        public class Builder
        {
            private readonly StandardHeader _header = new();
            
            public Builder SetParent(VisualElement parent)
            {
                _header._parent = parent ?? throw new ArgumentNullException(nameof(parent));
                return this;
            }

            public Builder SetTitle(string title)
            {
                _header._titleText = title;
                return this;
            }
            
            public Builder SetBackButton(Action action, string text = "<")
            {
                _header._backAction= action;
     
                return this;
            }

            public Builder SetQuitButton(Action action, string text = "X")
            {
                _header._quitAction = action;
              
                return this;
            }
            
            public Builder SetHeaderStyle(string style)
            {
                _header._headerStyle = style;
                return this;
            }

            public Builder SetTitleTextStyle(string style)
            {
                _header._titleTextStyle = style;
                return this;
            }

            public Builder SetButtonStyle(string style)
            {
                _header._buttonStyle = style;
                return this;
            }
            
            
            public StandardHeader Build()
            {
                if (_header._parent == null)
                    throw new InvalidOperationException("Parent must be set before building the footer.");

                _header.BuildHeader();
                return _header;
            }
        }

    }
}
