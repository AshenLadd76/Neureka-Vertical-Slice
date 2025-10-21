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
            VisualElement.text = text;
            return this;
        }

        public LabelBuilder SetFont(Font font)
        {
            if (font == null)
                throw new ArgumentNullException(nameof(font));

            // Apply the font dynamically
            VisualElement.style.unityFont = font;
            
            VisualElement.MarkDirtyRepaint(); 
            
            return this;
        }

        public LabelBuilder SetFontAsset(FontDefinition fontAsset)
        {
            VisualElement.style.unityFontDefinition = fontAsset;
            return this;
        }


        public LabelBuilder SetFontSize(float size)
        {
            VisualElement.style.fontSize = size;
            return this;
        }

        public LabelBuilder SetTextColor(Color color)
        {
            VisualElement.style.color = color;
            return this;
            
        }
        
    }
}
