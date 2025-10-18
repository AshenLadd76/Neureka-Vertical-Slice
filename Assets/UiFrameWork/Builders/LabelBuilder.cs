using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class LabelBuilder : BaseBuilder<Label, LabelBuilder>
    {
        public LabelBuilder SetText(string text)
        {
            _visualElement.text = text;
            return this;
        }

        public LabelBuilder SetFont(Font font)
        {
            if (font == null)
                throw new ArgumentNullException(nameof(font));

            // Apply the font dynamically
            _visualElement.style.unityFont = font;
            
            _visualElement.MarkDirtyRepaint(); 
            
            return this;
        }

        public LabelBuilder SetFontAsset(FontDefinition fontAsset)
        {
            _visualElement.style.unityFontDefinition = fontAsset;
            return this;
        }


        public LabelBuilder SetFontSize(float size)
        {
            _visualElement.style.fontSize = size;
            return this;
        }

        public LabelBuilder SetTextColor(Color color)
        {
            _visualElement.style.color = color;
            return this;
            
        }
        
    }
}
