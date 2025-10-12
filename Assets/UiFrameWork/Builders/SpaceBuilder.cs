using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class SpaceBuilder : BaseBuilder<VisualElement, SpaceBuilder>
    {
        public SpaceBuilder Horizontal(float width)
        {
            _visualElement.style.width = width;
            _visualElement.style.flexShrink = 0;
            _visualElement.style.flexGrow = 0;
            _visualElement.style.flexDirection = FlexDirection.Row;
            return this;
        }

        public SpaceBuilder Vertical(float height)
        {
            _visualElement.style.height = height;
            _visualElement.style.flexShrink = 0;
            _visualElement.style.flexGrow = 0;
            _visualElement.style.flexDirection = FlexDirection.Column;
            return this;
        }
    }
}