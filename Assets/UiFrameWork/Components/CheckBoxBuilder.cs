using System;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class CheckBoxBuilder : BaseBuilder<Toggle, CheckBoxBuilder>
    {
        public CheckBoxBuilder SetLabel(string text)
        {
            VisualElement.text = text;
            return this;
        }

        public CheckBoxBuilder SetValue(bool value)
        {
            VisualElement.value = value;
            
            return this;
        }

        public CheckBoxBuilder OnValueChanged(Action<bool> callback)
        {
            
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            VisualElement.RegisterValueChangedCallback(evt => callback(evt.newValue));
            
            return this;
        }
    }
}
