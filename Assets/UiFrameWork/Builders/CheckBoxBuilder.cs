using System;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class CheckBoxBuilder : BaseBuilder<Toggle, CheckBoxBuilder>
    {
        public CheckBoxBuilder SetLabel(string text)
        {
            _visualElement.text = text;
            return this;
        }

        public CheckBoxBuilder SetValue(bool value)
        {
            _visualElement.value = value;
            
            return this;
        }

        public CheckBoxBuilder OnValueChanged(Action<bool> callback)
        {
            
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            _visualElement.RegisterValueChangedCallback(evt => callback(evt.newValue));
            
            return this;
        }
    }
}
