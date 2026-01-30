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

      
        
        private readonly Queue<Vector2> _velocityHistory = new Queue<Vector2>();

    }
}
