using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Helpers
{
    public class SimplePointerHelper
    {
        private readonly VisualElement _target;
        private readonly Action<Vector2> _onDrag;
        private readonly Action _onDragEnd;
        private readonly Action _onClick;

        private Vector3 _startPosition;
        private bool _isDragging;
        private const float DragThreshold = 5f;

        public SimplePointerHelper(VisualElement target, Action<Vector2> onDrag = null, Action onDragEnd = null, Action onClick = null)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _onDrag = onDrag;
            _onDragEnd = onDragEnd;
            _onClick = onClick;

            _target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _target.RegisterCallback<PointerUpEvent>(OnPointerUp);
            _target.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            _startPosition = evt.position;
            _isDragging = false;
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (evt.pressedButtons == 0) return;

            var delta = evt.position - _startPosition;

            if (!_isDragging && delta.magnitude > DragThreshold)
                _isDragging = true;

            if (_isDragging)
                _onDrag?.Invoke(delta);
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (_isDragging)
                _onDragEnd?.Invoke();
            else
                _onClick?.Invoke();
        }

        private void OnPointerLeave(PointerLeaveEvent evt)
        {
            if (_isDragging)
                _onDragEnd?.Invoke();

            _isDragging = false;
        }
    }
}

