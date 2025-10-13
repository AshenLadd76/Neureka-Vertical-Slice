using System;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class ButtonBuilder : BaseBuilder<Button, ButtonBuilder>
    {
        public ButtonBuilder SetText(string text)
        {
            _visualElement.text = text;
            return this;
        }
        
        public ButtonBuilder OnClick(Action callback)
        {
            if (callback == null)
                throw new System.ArgumentNullException(nameof(callback));
            
            _visualElement.clicked += callback;
            return this;
        }
        
    }
}
