using System;
using System.Collections.Generic;
using CodeBase.UiComponents.Styles;
using FluentUI.Components;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.UiComponents.Factories
{
    public static class ButtonFactory
    {
        private static readonly Dictionary<ButtonType, ButtonConfiguration> ButtonConfigurations = new()
        {
            [ButtonType.Confirm] = new ButtonConfiguration
            {
                Text = "Confirm",
                StyleClasses = new[] { UiStyleClassDefinitions.ButtonBase, UiStyleClassDefinitions.ButtonLarge },
            },
            [ButtonType.Cancel] = new ButtonConfiguration
            {
                Text = "Cancel",
                StyleClasses = new[] { UiStyleClassDefinitions.ButtonBase, UiStyleClassDefinitions.ButtonSmall },
            }
        };

        public static Button CreateButton(ButtonType buttonType, string buttonText,  Action onClick, VisualElement parent = null)
        {
            if (!ButtonConfigurations.TryGetValue(buttonType, out var buttonConfiguration))
            {
                Logger.LogWarning($"No button configuration found for {buttonType}");
                return new ButtonBuilder().SetText("Please fix me").Build();
            }
            
            if( string.IsNullOrWhiteSpace( buttonText ) ) buttonText  = "Give me text !!";
            
            return new ButtonBuilder()
                .SetText(buttonText)
                .OnClick(onClick)
                .AddClasses(buttonConfiguration.StyleClasses)
                .AttachTo(parent)
                .Build();
        }
        
    }


    public enum ButtonType
    {
        Primary,
        Secondary,
        Confirm,
        Cancel,
    }

    public class ButtonConfiguration
    {
        public string Text;
        public string[] StyleClasses;
    
    }
}