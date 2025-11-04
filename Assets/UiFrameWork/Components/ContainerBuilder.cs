using System;
using UiFrameWork.Builders;
using UnityEngine.UIElements;

namespace UiFrameWork.Components
{
    public class ContainerBuilder : BaseBuilder<VisualElement, ContainerBuilder>
    {
        public ContainerBuilder SetMinWidth(int minWidth)
        {
            VisualElement.style.minWidth = minWidth;
            return this;
        }

        public ContainerBuilder SetMinHeight(int minHeight)
        {
            VisualElement.style.minHeight = minHeight;
            return this;
        }

        public ContainerBuilder SetPadding(int top, int left, int right, int bottom)
        {
            VisualElement.style.paddingTop = top;
            VisualElement.style.paddingRight = right;
            VisualElement.style.paddingBottom = bottom;
            VisualElement.style.paddingLeft = left;
            
            return this;
        }

        public ContainerBuilder SetMargin(int top, int left, int right, int bottom)
        {
            VisualElement.style.paddingTop = top;
            VisualElement.style.paddingRight = right;
            VisualElement.style.paddingBottom = left;
            VisualElement.style.paddingLeft = bottom;
            
            return this;
        }

        // public ContainerBuilder SetOnClick(Action onClick)
        // {
        //     if (onClick == null)
        //         throw new System.ArgumentNullException(nameof(onClick));
        //     
        //     VisualElement.RegisterCallback<ClickEvent>(evt =>
        //     {
        //         onClick?.Invoke();
        //     });
        //     return this;
        //     
        //
        // }
        
        
        // public PanelBuilder OnGeometryChanged(EventCallback<GeometryChangedEvent> callback)
        // {
        //     _visualElement.RegisterCallback(callback);
        //     return this;
        // }
    }
}