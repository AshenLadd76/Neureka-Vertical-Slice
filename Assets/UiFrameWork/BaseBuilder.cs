using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork
{
    public class BaseBuilder<TElement, TBuilder> : IUIBuilder<TElement, TBuilder>
        where TElement : VisualElement, new()
        where TBuilder : BaseBuilder<TElement, TBuilder>
    {
        protected TElement _visualElement = new TElement();

        public TBuilder SetWidth(float width) { _visualElement.style.width = width; return (TBuilder)this; }

        public TBuilder SetHeight(float height) { _visualElement.style.height = height; return (TBuilder)this; }
        
        public TBuilder SetWidthPercent(Length percent) { _visualElement.style.width = percent; return (TBuilder)this; }
        
        public TBuilder SetMinWidthPercent(Length percent) { _visualElement.style.minWidth = percent; return (TBuilder)this; }
        
        public TBuilder SetHeightPercent(Length percent) { _visualElement.style.height = percent; return (TBuilder)this; }
        
        public TBuilder SetMinHeightPercent(Length percent) { _visualElement.style.minHeight = percent; return (TBuilder)this; }

        public TBuilder SetBackgroundColor(Color backgroundColor){_visualElement.style.backgroundColor = backgroundColor; return (TBuilder)this; }

        public TBuilder AddClass(string className) { _visualElement.AddToClassList( className ); return (TBuilder)this; }

        public TBuilder RemoveClass(string className) { _visualElement.RemoveFromClassList( className ); return (TBuilder)this; }
        
        public TBuilder SetPickingMode(PickingMode mode) { _visualElement.pickingMode = mode; return (TBuilder)this; }


        public TBuilder OnGeometryChanged(EventCallback<GeometryChangedEvent> callback)
        {
            _visualElement.RegisterCallback(callback);
            return (TBuilder)this;
            
        }

        public TElement Build() => _visualElement;
        
        
        public TBuilder AttachTo(VisualElement parent)
        {
           parent.Add(_visualElement);
           return (TBuilder)this;
        }
        
        
        // 🆕 Add a single child element
        public TBuilder AddChild(VisualElement child)
        {
            _visualElement.Add(child);
            return (TBuilder)this;
        }

        // 🆕 Add a child builder (auto-builds its element)
        public TBuilder AddChild<TChildElement, TChildBuilder>(IUIBuilder<TChildElement, TChildBuilder> childBuilder) where TChildElement : VisualElement where TChildBuilder : IUIBuilder<TChildElement, TChildBuilder>
        {
            _visualElement.Add( childBuilder.Build() );
            return (TBuilder)this;
        }

        // Optional helper for multiple children at once
        public TBuilder AddChildren<TChildElement, TChildBuilder>(IEnumerable<IUIBuilder<TChildElement, TChildBuilder>> childBuilders) where TChildElement : VisualElement where TChildBuilder : IUIBuilder<TChildElement, TChildBuilder>
        {
            foreach( var child in childBuilders )
            {
                _visualElement.Add( child.Build() );
            }
            return (TBuilder)this;
        }

        public TBuilder SetPadding(float left, float right, float top, float bottom)
        {
            _visualElement.style.paddingLeft = left;
            _visualElement.style.paddingRight = right;
            _visualElement.style.paddingTop = top;
            _visualElement.style.paddingBottom = bottom;
            
            return (TBuilder)this;
        }

        public TBuilder SetMinMax(float minWidth, float minHeight, float maxWidth, float maxHeight)
        {
            _visualElement.style.minWidth = minWidth;
            _visualElement.style.minHeight = minHeight;
            _visualElement.style.maxWidth = maxWidth;
            _visualElement.style.maxHeight = maxHeight;
            
            return (TBuilder)this;
        }

        public TBuilder SetFixedSize(float width, float height)
        { 
            _visualElement.style.width = width;
            _visualElement.style.height = height;
            
            return (TBuilder)this;
        }

        public TBuilder SetBorder(float width)
        {
            _visualElement.style.borderTopWidth = width;
            _visualElement.style.borderRightWidth = width;
            _visualElement.style.borderBottomWidth = width;
            _visualElement.style.borderLeftWidth = width;
            
            return (TBuilder)this;
        }

        public TBuilder SetBorderColor(Color color)
        {
            _visualElement.style.borderTopColor = color;
            _visualElement.style.borderRightColor = color;
            _visualElement.style.borderBottomColor = color;
            _visualElement.style.borderLeftColor = color;

            return (TBuilder)this;
        }

        public TBuilder SetBorderRadius(float radius)
        {
            _visualElement.style.borderTopLeftRadius = radius;
            _visualElement.style.borderTopRightRadius = radius;
            _visualElement.style.borderBottomLeftRadius = radius;
            _visualElement.style.borderBottomRightRadius = radius;
            
            return (TBuilder)this;
        }

        
        
        //Flex 
        
        public TBuilder SetFlexDirection(FlexDirection direction)
        {
            _visualElement.style.flexDirection = direction;
            return (TBuilder)this;
        }

        public TBuilder SetAlignItems(Align align)
        {
            _visualElement.style.alignItems = align;
            return (TBuilder)this;
        }

        public TBuilder SetJustifyContent(Justify justify)
        {
            _visualElement.style.justifyContent = justify;
            return (TBuilder)this;
        }

        public TBuilder SetFlexGrow(float flexGrow)
        {
            _visualElement.style.flexGrow = flexGrow;
            return (TBuilder)this;
        }

        public TBuilder SetFlexShrink(float flexShrink)
        {
            _visualElement.style.flexShrink = flexShrink;
            return (TBuilder)this;
        }

        public TBuilder SetFlexBasis(float flexBasis)
        {
            _visualElement.style.flexBasis = flexBasis;
            return (TBuilder)this;
        }
        
        //Flex Ends
        
        //Safe Area 
        public TBuilder ApplySafeArea()
        {
            var safe = Screen.safeArea;
            _visualElement.style.paddingTop = safe.yMin;
            _visualElement.style.paddingBottom = Screen.height - safe.yMax;
            return (TBuilder)this;
        }
        //Safe Area Ends
        
        
        //Generic style setter
        public TBuilder SetStyle(System.Action<VisualElement> configure)
        {
            configure?.Invoke(_visualElement);
            return (TBuilder)this;
        }

        //Generic style setter ends
    }
}
