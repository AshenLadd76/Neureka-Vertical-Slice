using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class ScrollViewBuilder : BaseBuilder<ScrollView, ScrollViewBuilder>
    {
        public ScrollViewBuilder SetOrientation(ScrollViewMode mode)
        {
            VisualElement.mode = mode;
            return this;
        }
        
        public ScrollViewBuilder SetContentSize(float width, float height)
        {
            VisualElement.contentContainer.style.width = width;
            VisualElement.contentContainer.style.height = height;
            return this;
        }

        // Add a child element to the scroll view
        public ScrollViewBuilder AddElement(VisualElement element)
        {
            VisualElement.Add(element);
            return this;
        }
        
        public ScrollViewBuilder OnScroll(Action<Vector2> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            VisualElement.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                callback(VisualElement.scrollOffset);
            });

            return this;
        }
        
        // Enable or disable scrolling in each direction
        public ScrollViewBuilder HideScrollBars(ScrollerVisibility scrollerVisibilityVertical, ScrollerVisibility scrollerVisibilityHorizontal )
        {

            VisualElement.verticalScrollerVisibility = scrollerVisibilityVertical;
            VisualElement.horizontalScrollerVisibility = scrollerVisibilityHorizontal;
            
            
            return this;
        }
        
    }
}
