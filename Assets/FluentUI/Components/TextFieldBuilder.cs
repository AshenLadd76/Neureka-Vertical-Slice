using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class TextFieldBuilder : FluentUI.Builders.BaseBuilder<TextField, TextFieldBuilder>
    {
        public TextFieldBuilder SetText(string text)
        {
            VisualElement.value = text;

            return this;
        }

        public TextFieldBuilder SetMultiline(bool multiline, string ussClassName = null)
        {
            VisualElement.multiline = multiline;

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
            
            VisualElement.RegisterValueChangedCallback( evt => callback(evt.newValue) );
            
            return this;
        }

        public TextFieldBuilder SetFont(Font font)
        {
            VisualElement.style.unityFont = font;
            return this;
        }

        public TextFieldBuilder SetFontSize(float fontSize)
        {
            VisualElement.style.fontSize = fontSize;
            return this;
        }
    }
}
