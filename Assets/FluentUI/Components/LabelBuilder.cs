using System;
using UiFrameWork.Builders;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Components
{
    public class LabelBuilder : BaseBuilder<Label, LabelBuilder>
    {
        public LabelBuilder SetText(string text)
        {
            VisualElement.text = text ?? string.Empty;
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

        public LabelBuilder SetTextAlignment(TextAnchor alignment)
        {
            VisualElement.style.unityTextAlign = alignment;
            return this;
        }
        
        public LabelBuilder SetVisibility(Visibility visibility)
        {
            VisualElement.style.visibility = visibility;
            return this;
            
        }
        
        public void Hide(bool hide) => VisualElement.style.display = hide ? DisplayStyle.None : DisplayStyle.Flex;
        
    }
}
