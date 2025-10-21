using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public partial class BaseBuilder<TElement, TBuilder> 
    {
        
        #region Layout & Styling
        
        public TBuilder SetWidth(float width) { VisualElement.style.width = width; return (TBuilder)this; }

        public TBuilder SetHeight(float height) { VisualElement.style.height = height; return (TBuilder)this; }
        
        public TBuilder SetWidthPercent(Length percent) { VisualElement.style.width = percent; return (TBuilder)this; }
        
        public TBuilder SetMinWidthPercent(Length percent) { VisualElement.style.minWidth = percent; return (TBuilder)this; }
        
        public TBuilder SetHeightPercent(Length percent) { VisualElement.style.height = percent; return (TBuilder)this; }
        
        public TBuilder SetMinHeightPercent(Length percent) { VisualElement.style.minHeight = percent; return (TBuilder)this; }

        public TBuilder SetBackgroundColor(Color backgroundColor){VisualElement.style.backgroundColor = backgroundColor; return (TBuilder)this; }
        
        
        public TBuilder SetPickingMode(PickingMode mode) { VisualElement.pickingMode = mode; return (TBuilder)this; }

     
        public TBuilder OnGeometryChanged(EventCallback<GeometryChangedEvent> callback)
        {
            VisualElement.RegisterCallback(callback);
            return (TBuilder)this;
        }
        
        public TBuilder SetPadding(float left, float right, float top, float bottom)
        {
            VisualElement.style.paddingLeft = left;
            VisualElement.style.paddingRight = right;
            VisualElement.style.paddingTop = top;
            VisualElement.style.paddingBottom = bottom;
            
            return (TBuilder)this;
        }

        public TBuilder SetMargin(float left, float top, float right, float bottom)
        {
            VisualElement.style.marginLeft = left;
            VisualElement.style.marginRight = right;
            VisualElement.style.marginTop = top;
            VisualElement.style.marginBottom = bottom;
            return (TBuilder)this;
        }

        public TBuilder SetMinMax(float minWidth, float minHeight, float maxWidth, float maxHeight)
        {
            VisualElement.style.minWidth = minWidth;
            VisualElement.style.minHeight = minHeight;
            VisualElement.style.maxWidth = maxWidth;
            VisualElement.style.maxHeight = maxHeight;
            
            return (TBuilder)this;
        }

        public TBuilder SetFixedSize(float width, float height)
        { 
            VisualElement.style.width = width;
            VisualElement.style.height = height;
            
            return (TBuilder)this;
        }

        public TBuilder SetBorder(float width)
        {
            VisualElement.style.borderTopWidth = width;
            VisualElement.style.borderRightWidth = width;
            VisualElement.style.borderBottomWidth = width;
            VisualElement.style.borderLeftWidth = width;
            
            return (TBuilder)this;
        }

        public TBuilder SetBorderColor(Color color)
        {
            VisualElement.style.borderTopColor = color;
            VisualElement.style.borderRightColor = color;
            VisualElement.style.borderBottomColor = color;
            VisualElement.style.borderLeftColor = color;

            return (TBuilder)this;
        }

        public TBuilder SetBorderRadius(float radius)
        {
            VisualElement.style.borderTopLeftRadius = radius;
            VisualElement.style.borderTopRightRadius = radius;
            VisualElement.style.borderBottomLeftRadius = radius;
            VisualElement.style.borderBottomRightRadius = radius;
            
            return (TBuilder)this;
        }
        
        #endregion



        #region Flex
        
        public TBuilder SetFlexDirection(FlexDirection direction)
        {
            VisualElement.style.flexDirection = direction;
            return (TBuilder)this;
        }

        public TBuilder SetAlignItems(Align align)
        {
            VisualElement.style.alignItems = align;
            return (TBuilder)this;
        }

        public TBuilder SetJustifyContent(Justify justify)
        {
            VisualElement.style.justifyContent = justify;
            return (TBuilder)this;
        }

        public TBuilder SetFlexGrow(float flexGrow)
        {
            VisualElement.style.flexGrow = flexGrow;
            return (TBuilder)this;
        }

        public TBuilder SetFlexShrink(float flexShrink)
        {
            VisualElement.style.flexShrink = flexShrink;
            return (TBuilder)this;
        }

        public TBuilder SetFlexBasis(float flexBasis)
        {
            VisualElement.style.flexBasis = flexBasis;
            return (TBuilder)this;
        }
        
        #endregion
        
        //Safe Area 
        public TBuilder ApplySafeArea()
        {
            var safe = Screen.safeArea;
            VisualElement.style.paddingTop = safe.yMin;
            VisualElement.style.paddingBottom = Screen.height - safe.yMax;
            return (TBuilder)this;
        }
        //Safe Area Ends
        
        
        //Generic style setter
        public TBuilder SetStyle(System.Action<VisualElement> configure)
        {
            configure?.Invoke(VisualElement);
            return (TBuilder)this;
        }

        //Generic style setter ends
    }
}
