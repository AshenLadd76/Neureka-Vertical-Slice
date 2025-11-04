using System;
using System.Collections.Generic;
using UiFrameWork.Builders;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

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

        public ScrollViewBuilder EnableInertia(bool enable = true, float deceleration = 5f)
        {
            if (!enable) return this;

            _deceleration = deceleration;
            
            VisualElement.RegisterCallback<PointerDownEvent>(evt =>
            {
                _isDragging = true;
                _lastPointerPosition = evt.position;
                _velocity = Vector2.zero;
                _velocityHistory.Clear();
                
                VisualElement.CapturePointer(evt.pointerId);
                
                Logger.Log( $"Pointer Id: {evt.pointerId}" );
            });
            
            VisualElement.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!_isDragging) return;

                Vector2 delta = evt.position - _lastPointerPosition;
                _lastPointerPosition = evt.position;
                
                
                var offset = new Vector2(delta.x, +delta.y);
                
                VisualElement.scrollOffset -= offset;
                
                //Record velocity
                Vector2 frameVelocity = offset / Time.deltaTime;
                
                _velocityHistory.Enqueue(frameVelocity);
                
                if (_velocityHistory.Count > _maxHistoryFrames) _velocityHistory.Dequeue();
                
            });
            
            VisualElement.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (!_isDragging) return;
                
                _isDragging = false;
                VisualElement.ReleasePointer(evt.pointerId);
                
                //compute average velocity from last N frames
                
                Vector2 sum = Vector2.zero;

                foreach (var v in _velocityHistory)
                {
                    sum += v;
                }
                
                if (_velocityHistory.Count > 0)
                    _velocity = sum / _velocityHistory.Count;
                
                _velocityHistory.Clear();
                
            });
            
            VisualElement.schedule.Execute(UpdateInertia).Every(16);
            
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
        
        private void UpdateInertia()
        {
            if (_isDragging || _velocity.sqrMagnitude < 0.01f) return;

            VisualElement.scrollOffset -= _velocity * Time.deltaTime;
            _velocity = Vector2.Lerp(_velocity, Vector2.zero, _deceleration * Time.deltaTime);
        }
        
    }
}
