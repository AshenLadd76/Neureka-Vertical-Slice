using UnityEngine.UIElements;

namespace UiFrameWork
{
    public interface IUIBuilderEvents<TElement, TBuilder>
        where TElement : VisualElement
        where TBuilder : IUIBuilderEvents<TElement, TBuilder>
    {
        TBuilder OnClick(EventCallback<ClickEvent> callback);
        TBuilder OnMouseEnter(EventCallback<MouseEnterEvent> callback);
        TBuilder OnMouseLeave(EventCallback<MouseLeaveEvent> callback);
        TBuilder OnMouseDown(EventCallback<MouseDownEvent> callback);
        TBuilder OnMouseUp(EventCallback<MouseUpEvent> callback);
        TBuilder OnGeometryChanged(EventCallback<GeometryChangedEvent> callback);
        
        
#if UNITY_EDITOR        
        TBuilder OnDragEnter(EventCallback<DragEnterEvent> callback);
        TBuilder OnDragLeave(EventCallback<DragLeaveEvent> callback);
        TBuilder OnDrop(EventCallback<DragPerformEvent> callback);
        
#endif        
      
    }
}