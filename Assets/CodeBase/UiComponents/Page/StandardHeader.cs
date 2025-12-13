using System;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Page
{
    public class StandardHeader
    {
        private Action _backAction;
        private Action _quitAction;
        
        private string _titleText = "Standard Header";
        
        private string _backButtonText  = "<";
        private string _quitButtonText  = "X";
        private VisualElement _parent;
       
        private string _headerStyle = "header-nav";
        private string _buttonStyle = "demo-header-button";
        private string _headerTitleContainerStyle = "header-title";
       
        
        private string _titleTextStyle = "header-label";

        public VisualElement _root { get; private set; }

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

            _backButton = new ButtonBuilder().SetText(_backButtonText)
                .OnClick(() =>
                {
                    _backAction?.Invoke();
                })
                .AddClass(_buttonStyle)
                .AddClass(UiStyleClassDefinitions.HeaderLabel)
                .AttachTo(headerNav)
                .Build();
            
            new ContainerBuilder().AttachTo(headerNav).AddClass("header-spacer").Build();
            
            new ButtonBuilder().SetText(_quitButtonText)
                .OnClick(() =>
                {
                    _quitAction?.Invoke();
                    
                })
                .AddClass(_buttonStyle)
                .AddClass(UiStyleClassDefinitions.HeaderLabel)
                .AttachTo(headerNav)
                .Build();
            
            var headerTitle =  new ContainerBuilder().AddClass(_headerTitleContainerStyle).AttachTo(_parent).Build();

            var label = new LabelBuilder().SetText(_titleText).AddClass(_titleTextStyle).AttachTo(headerTitle).Build();
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
                _header._backButtonText = text;
                return this;
            }

            public Builder SetQuitButton(Action action, string text = "X")
            {
                _header._quitAction = action;
                _header._quitButtonText = text;
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
