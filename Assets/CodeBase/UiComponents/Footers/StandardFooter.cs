using System;
using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Footers
{
    public class StandardFooter
    {
        private Action _primaryAction;
        private Action _secondaryAction;
        
        private string _primaryText = "Continue";
        private string _secondaryText = "Back";
        private VisualElement _parent;
        
        private string _footerStyle = "questionnaire-footer";
        private string _buttonStyle  = "questionnaire-footer-button";

        private VisualElement _root;

        private void BuildFooter()
        {
            _root = new ContainerBuilder()
                .AddClass(_footerStyle)
                .AttachTo(_parent)
                .Build();

            if (_primaryAction != null)
            {
                new ButtonBuilder()
                    .SetText(_primaryText)
                    .AddClass(_buttonStyle)
                    .OnClick(() =>
                    {
                        _primaryAction.Invoke();
                    })
                    .AttachTo(_root)
                    .Build();
            }

            if (_secondaryAction != null)
            {
                new ButtonBuilder()
                    .SetText(_secondaryText)
                    .AddClass(_buttonStyle)
                    .OnClick(() =>
                    {
                        _secondaryAction.Invoke();
                    })
                    .AttachTo(_root)
                    .Build();
            }
        }

        // Builder nested class
        public class Builder
        {
            private readonly StandardFooter _footer = new();

            public Builder SetParent(VisualElement parent)
            {
                _footer._parent = parent ?? throw new ArgumentNullException(nameof(parent));
                return this;
            }

            public Builder SetPrimaryButton(Action action, string text = "Continue")
            {
                _footer._primaryAction = action;
                _footer._primaryText = text;
                return this;
            }

            public Builder SetSecondaryButton(Action action, string text = "Back")
            {
                _footer._secondaryAction = action;
                _footer._secondaryText = text;
                return this;
            }

            public Builder SetFooterStyle(string style)
            {
                _footer._footerStyle = style;
                return this;
            }

            public Builder SetButtonStyle(string style)
            {
                _footer._buttonStyle = style;
                return this;
            }

            public StandardFooter Build()
            {
                if (_footer._parent == null)
                    throw new InvalidOperationException("Parent must be set before building the footer.");

                _footer.BuildFooter();
                return _footer;
            }
        }
    }
}


