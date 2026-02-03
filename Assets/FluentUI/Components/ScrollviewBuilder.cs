using System;
using System.Collections.Generic;
using UiFrameWork.Builders;
using UiFrameWork.Helpers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Components
{
    public class ScrollViewBuilder : BaseBuilder<ScrollView, ScrollViewBuilder>
    {
        private Vector3 _lastPointerPosition;
        private Vector2 _velocity;
        
        private bool _isDragging;

        private float _deceleration = 10f;
        
        private readonly Queue<Vector2> _velocityHistory = new Queue<Vector2>();
        private readonly int _maxHistoryFrames = 10;

        public ScrollViewBuilder EnableInertia(bool enable = true, float deceleration = 5)
        {
            if (!enable) return this;

            _deceleration = deceleration;
            
            new ScrollViewDragHelper(VisualElement, _deceleration );
            
            return this;
        }

        
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

        public ScrollViewBuilder SetDecelerationRate(float decelerationRate)
        {
            VisualElement.scrollDecelerationRate = decelerationRate;
            return this;
        }

        // Add a child element to the scroll view
        public ScrollViewBuilder AddElement(VisualElement element)
        {
            VisualElement.Add(element);
            return this;
        }
        
        
        //Add multiple child elements to the scroll view.
        public ScrollViewBuilder AddElements(IEnumerable<VisualElement> elements)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));

            foreach (var element in elements)
            {
                VisualElement.Add(element);
            }

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
