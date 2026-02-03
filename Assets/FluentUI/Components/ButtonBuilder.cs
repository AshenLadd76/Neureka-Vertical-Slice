using ToolBox.Services.Haptics;
using UnityEngine.UIElements;

namespace FluentUI.Components
{
    public class ButtonBuilder : Builders.BaseBuilder<Button, ButtonBuilder>
    {
        private HapticType _hapticType;
        
        public ButtonBuilder SetText(string text)
        {
            VisualElement.text = text;
            return this;
        }
    }
}
