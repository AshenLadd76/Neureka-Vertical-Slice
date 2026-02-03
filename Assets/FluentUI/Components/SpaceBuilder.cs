using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class SpaceBuilder : FluentUI.Builders.BaseBuilder<VisualElement, SpaceBuilder>
    {
        public SpaceBuilder Horizontal(float width)
        {
            VisualElement.style.width = width;
            VisualElement.style.flexShrink = 0;
            VisualElement.style.flexGrow = 0;
            VisualElement.style.flexDirection = FlexDirection.Row;
            return this;
        }

        public SpaceBuilder Vertical(float height)
        {
            VisualElement.style.height = height;
            VisualElement.style.flexShrink = 0;
            VisualElement.style.flexGrow = 0;
            VisualElement.style.flexDirection = FlexDirection.Column;
            return this;
        }
    }
}