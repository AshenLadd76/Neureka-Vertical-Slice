using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class ScrollViewBuilder : BaseBuilder<ScrollView, ScrollViewBuilder>
    {
        public ScrollViewBuilder SetOrientation(ScrollViewMode mode)
        {
            _visualElement.mode = mode;
            return this;
        }
        
        public ScrollViewBuilder SetContentSize(float width, float height)
        {
            _visualElement.contentContainer.style.width = width;
            _visualElement.contentContainer.style.height = height;
            return this;
        }

        // Add a child element to the scroll view
        public ScrollViewBuilder AddElement(VisualElement element)
        {
            _visualElement.Add(element);
            return this;
        }
        
        public ScrollViewBuilder OnScroll(Action<Vector2> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            _visualElement.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                callback(_visualElement.scrollOffset);
            });

            return this;
        }
        
        // Enable or disable scrolling in each direction
        public ScrollViewBuilder SetScrolling(bool enableVertical = true, bool enableHorizontal = false)
        {
            if (_visualElement.verticalScroller != null)
                _visualElement.verticalScroller.style.display = enableVertical ? DisplayStyle.Flex : DisplayStyle.None;

            if (_visualElement.horizontalScroller != null)
                _visualElement.horizontalScroller.style.display = enableHorizontal ? DisplayStyle.Flex : DisplayStyle.None;

            return this;
        }
        
    }
}
