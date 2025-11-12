using System;
using UiFrameWork.Builders;
using UnityEngine.UIElements;

namespace UiFrameWork.Components
{
    public class ButtonBuilder : BaseBuilder<Button, ButtonBuilder>
    {
        public ButtonBuilder SetText(string text)
        {
            VisualElement.text = text;
            return this;
        }
        
        
        // public ButtonBuilder OnClick(Action callback)
        // {
        //     if (callback == null)
        //         throw new System.ArgumentNullException(nameof(callback));
        //     
        //     VisualElement.clicked += callback;
        //     return this;
        // }

        
        
    }
}
