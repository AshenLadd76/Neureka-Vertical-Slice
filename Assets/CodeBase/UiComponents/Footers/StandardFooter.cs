using System;
using ToolBox.Services.Haptics;
using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Footers
{
    public class StandardFooter
    {
        private Action PrimaryAction { get; set; }
        private Action SecondaryAction { get; set; }
        private string PrimaryText { get; set; } = "Continue";
        private string SecondaryText { get; set; } = "Back";
        private VisualElement Parent { get; set; } = null!;
        private string FooterStyle { get; set; } = "questionnaire-footer";
        private string ButtonStyle { get; set; } = "questionnaire-footer-button";

        public VisualElement Root { get; private set; }

        private void BuildFooter()
        {
            Root = new ContainerBuilder()
                .AddClass(FooterStyle)
                .AttachTo(Parent)
                .Build();

            if (PrimaryAction != null)
            {
                new ButtonBuilder()
                    .SetText(PrimaryText)
                    .AddClass(ButtonStyle)
                    .OnClick(() =>
                    {
                        HapticsHelper.RequestHaptics(HapticType.Low);
                        PrimaryAction.Invoke();
                    })
                    .AttachTo(Root)
                    .Build();
            }

            if (SecondaryAction != null)
            {
                new ButtonBuilder()
                    .SetText(SecondaryText)
                    .AddClass(ButtonStyle)
                    .OnClick(() =>
                    {
                        HapticsHelper.RequestHaptics(HapticType.Low);
                        SecondaryAction.Invoke();
                    })
                    .AttachTo(Root)
                    .Build();
            }
        }

        // Builder nested class
        public class Builder
        {
            private readonly StandardFooter _footer = new();

            public Builder SetParent(VisualElement parent)
            {
                _footer.Parent = parent ?? throw new ArgumentNullException(nameof(parent));
                return this;
            }

            public Builder SetPrimaryButton(Action action, string text = "Continue")
            {
                _footer.PrimaryAction = action;
                _footer.PrimaryText = text;
                return this;
            }

            public Builder SetSecondaryButton(Action action, string text = "Back")
            {
                _footer.SecondaryAction = action;
                _footer.SecondaryText = text;
                return this;
            }

            public Builder SetFooterStyle(string style)
            {
                _footer.FooterStyle = style;
                return this;
            }

            public Builder SetButtonStyle(string style)
            {
                _footer.ButtonStyle = style;
                return this;
            }

            public StandardFooter Build()
            {
                if (_footer.Parent == null)
                    throw new InvalidOperationException("Parent must be set before building the footer.");

                _footer.BuildFooter();
                return _footer;
            }
        }
    }
}


