using System;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class TextFieldBuilder : BaseBuilder<TextField, TextFieldBuilder>
    {
        public TextFieldBuilder SetText(string text)
        {
            _visualElement.value = text;

            return this;
        }

        public TextFieldBuilder SetMultiline(bool multiline, string ussClassName = null)
        {
            _visualElement.multiline = multiline;

            if (!string.IsNullOrEmpty(ussClassName))
            {
                if (multiline)
                    AddClass(ussClassName);
                else
                    RemoveClass(ussClassName);
            }

            return this;
        }


        public TextFieldBuilder OnValueChanged(Action<string> callback)
        {
            if (callback == null)
                throw new System.ArgumentNullException(nameof(callback));
            
            _visualElement.RegisterValueChangedCallback( evt => callback(evt.newValue) );
            
            return this;
        }
    }
}
