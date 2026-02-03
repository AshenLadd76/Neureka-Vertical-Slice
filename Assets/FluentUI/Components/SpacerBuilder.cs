using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class SpacerBuilder : BaseBuilder<VisualElement, SpacerBuilder>
    {
        // Fixed size spacer
        public SpacerBuilder SetSize(float width = 0, float height = 0)
        {
            if (width > 0) VisualElement.style.width = width;
            if (height > 0) VisualElement.style.height = height;
            return this;
        }

        // Flexible spacer that grows to fill available space
        public SpacerBuilder SetFlexible()
        {
            VisualElement.style.flexGrow = 1;
            return this;
        }
    }
}
