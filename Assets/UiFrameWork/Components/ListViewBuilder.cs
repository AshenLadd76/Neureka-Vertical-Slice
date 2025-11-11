using System.Collections.Generic;
using UiFrameWork.Builders;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Components
{
    public class ListViewBuilder : BaseBuilder<ListView, ListViewBuilder>
    {
        private Vector3 _lastPointerPosition;
        private Vector2 _velocity;
        
        private bool _isDragging;

        private float _deceleration = 10f;
        
        private readonly Queue<Vector2> _velocityHistory = new Queue<Vector2>();
        private readonly int _maxHistoryFrames = 10;
    }
}
