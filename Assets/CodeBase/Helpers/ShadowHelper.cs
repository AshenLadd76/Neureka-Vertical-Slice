using System;
using System.Drawing;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

namespace CodeBase.Helpers
{
    public class ShadowHelper
    {
        public static VisualElement AddShadow(VisualElement visualElement, float offsetX, float offsetY)
        {
            if (visualElement == null) throw new ArgumentNullException(nameof(visualElement));
            
            var shadow = new ContainerBuilder().AddClass(UiStyleClassDefinitions.ImageShadow).Build();
            
            shadow.style.position = Position.Absolute;
            shadow.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 0.3f));
            
            shadow.style.borderTopLeftRadius = visualElement.style.borderTopLeftRadius;
            shadow.style.borderTopRightRadius = visualElement.style.borderTopRightRadius;
            shadow.style.borderBottomLeftRadius = visualElement.style.borderBottomLeftRadius;
            shadow.style.borderBottomRightRadius = visualElement.style.borderBottomRightRadius;
            

            // Match image size
            shadow.style.width = visualElement.style.width;
            shadow.style.height = visualElement.style.height;

            // Apply offset
            shadow.style.left = offsetX;
            shadow.style.top = offsetY;

            // Insert as first child so it appears behind the image
            visualElement.Insert(0, shadow);

            return shadow;
        }
    }
}
