using System;
using CodeBase.UiComponents.Styles;
using FluentUI.Components;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.Documents.DemoA.Components
{
    public class PrimaryButton
    {
        public VisualElement Button { get; private set; }

        public PrimaryButton(VisualElement parent, string buttonText, Action onClick)
        {
            // Validate parameters
            if (parent == null)
                throw new ArgumentNullException(nameof(parent), "Parent container cannot be null.");

            if (string.IsNullOrWhiteSpace(buttonText))
                throw new ArgumentException("Button text cannot be null or empty.", nameof(buttonText));

            if (onClick == null)
                throw new ArgumentNullException(nameof(onClick), "Button action cannot be null.");

            Button = BuildButton(parent, buttonText, onClick);
        }

        private VisualElement BuildButton(VisualElement parent, string buttonText, Action onClick)
        {
            return new ButtonBuilder()
                .SetText(buttonText)
                .AddClass(UiStyleClassDefinitions.SharedFooterButton)
                .OnClick(onClick)
                .AttachTo(parent)
                .Build();
        }
    }
}

