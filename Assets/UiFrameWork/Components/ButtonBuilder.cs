using System;
using ToolBox.Services.Haptics;
using UiFrameWork.Builders;
using UnityEngine.UIElements;

namespace UiFrameWork.Components
{
    public class ButtonBuilder : BaseBuilder<Button, ButtonBuilder>
    {
        private HapticType _hapticType;
        
        public ButtonBuilder SetText(string text)
        {
            VisualElement.text = text;
            return this;
        }

        public ButtonBuilder SetHaptics(HapticType hapticType)
        {
            _hapticType = hapticType;
            return this;
        }
        
        
        // public override ButtonBuilder OnClick(Action callback)
        // {
        //     base.OnClick(callback);
        //     
        //     
        //
        //     return this;
        // }

        
        
    }
}
