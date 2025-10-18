using System;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class ContainerBuilder : BaseBuilder<VisualElement, ContainerBuilder>
    {
        public ContainerBuilder SetMinWidth(int minWidth)
        {
            _visualElement.style.minWidth = minWidth;
            return this;
        }

        public ContainerBuilder SetMinHeight(int minHeight)
        {
            _visualElement.style.minHeight = minHeight;
            return this;
        }

        public ContainerBuilder SetPadding(int top, int left, int right, int bottom)
        {
            _visualElement.style.paddingTop = top;
            _visualElement.style.paddingRight = right;
            _visualElement.style.paddingBottom = bottom;
            _visualElement.style.paddingLeft = left;
            
            return this;
        }

        public ContainerBuilder SetMargin(int top, int left, int right, int bottom)
        {
            _visualElement.style.paddingTop = top;
            _visualElement.style.paddingRight = right;
            _visualElement.style.paddingBottom = left;
            _visualElement.style.paddingLeft = bottom;
            
            return this;
        }

        public ContainerBuilder SetOnClick(Action onClick)
        {
            if (onClick == null)
                throw new System.ArgumentNullException(nameof(onClick));
            
            _visualElement.RegisterCallback<ClickEvent>(evt =>
            {
                onClick?.Invoke();
            });
            return this;
            

        }
        
        
        // public PanelBuilder OnGeometryChanged(EventCallback<GeometryChangedEvent> callback)
        // {
        //     _visualElement.RegisterCallback(callback);
        //     return this;
        // }
    }
}