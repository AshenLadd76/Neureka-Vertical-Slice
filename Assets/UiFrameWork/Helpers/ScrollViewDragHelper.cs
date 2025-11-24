using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Helpers
{
    public class ScrollViewDragHelper
    {
        private readonly ScrollView _target;
        private readonly Action<Vector2> _onDrag;
        private readonly Action _onDragEnd;

        private Vector3 _lastPointerPosition;
        private bool _isDragging;
        private readonly Queue<Vector2> _velocityHistory = new Queue<Vector2>();
        private readonly int _maxHistoryFrames = 10;
        
        private readonly float _deceleration = 10f;

        public Vector2 Velocity { get; private set; } = Vector2.zero;

        public ScrollViewDragHelper(ScrollView target, float deceleration = 0f)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            
            if( deceleration > 0 ) _deceleration = deceleration;
          
            RegisterCallbacks();

            _target.schedule.Execute(UpdateInertia).Every(1);
        }

        private void RegisterCallbacks()
        {
            _target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _target.RegisterCallback<PointerUpEvent>(OnPointerUp);
            _target.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (!CanScroll) return;
            
            _isDragging = true;
            _lastPointerPosition = evt.position;
            _velocityHistory.Clear();
            Velocity = Vector2.zero;
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_isDragging || !CanScroll ) return;

            Vector2 delta = evt.position - _lastPointerPosition;
            _lastPointerPosition = evt.position;
            _target.scrollOffset -= delta;

            // track velocity
            Vector2 frameVelocity = delta / Time.deltaTime;
            _velocityHistory.Enqueue(frameVelocity);
            if (_velocityHistory.Count > _maxHistoryFrames) _velocityHistory.Dequeue();
        }

        private void OnPointerUp(PointerUpEvent evt) => CancelDrag();

        private void OnPointerLeave(PointerLeaveEvent evt) => CancelDrag();


        private void CancelDrag()
        {
            if (!_isDragging) return;

            _isDragging = false;

            // compute average velocity
            Vector2 sum = Vector2.zero;
            foreach (var v in _velocityHistory) sum += v;
            Velocity = _velocityHistory.Count > 0 ? sum / _velocityHistory.Count : Vector2.zero;

            _velocityHistory.Clear();
        }
        
        private void UpdateInertia()
        {
            if (_isDragging || Velocity.sqrMagnitude < 0.01f) return;

            _target.scrollOffset -= Velocity * Time.deltaTime;

            Velocity = Vector2.Lerp(Velocity, Vector2.zero, _deceleration * Time.deltaTime);
        }
        
        private bool CanScroll =>
            _target.contentContainer.resolvedStyle.height > _target.contentViewport.resolvedStyle.height;
    }
}
