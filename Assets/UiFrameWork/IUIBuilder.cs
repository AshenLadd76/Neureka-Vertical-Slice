using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;

namespace UiFrameWork
{
    /// <summary>
    /// A generic fluent interface for building UI Toolkit elements.
    /// Curiously Recurring Template Pattern (CRTP).
    /// That second constraint lets the compiler understand that when we’re inside the interface or base class, returning TBuilder means “return whatever the real builder is.”
    /// compilers way of letting you return this fluently and safely
    /// </summary>
    /// <typeparam name="TElement">The type of VisualElement being built.</typeparam>
    /// <typeparam name="TBuilder">The concrete builder type for fluent chaining.</typeparam>

    
    public interface IUIBuilder<TElement, TBuilder>
    where TElement : VisualElement
    where TBuilder : IUIBuilder<TElement, TBuilder>
    {
        
        /// <summary>
        /// manually set the width of the element
        /// </summary>
        TBuilder SetWidth( float width );

        public TBuilder SetMinWidthPercent(Length percent);
        
        
        /// <summary>
        /// manually set the height of the element
        /// </summary>
        TBuilder SetHeight( float height );

        public TBuilder SetMinHeightPercent(Length percent);

        /// <summary>
        /// manually set the percentage width of the element
        /// </summary>
        public TBuilder SetWidthPercent(Length percent);

        /// <summary>
        /// manually set the percentage height of the element
        /// </summary>
        public TBuilder SetHeightPercent(Length percent);
        
        /// <summary>
        /// Set the background colour if applicable
        /// </summary>
        TBuilder SetBackgroundColor( Color backgroundColor );

        
        /// <summary>
        /// Decide if you want to allow click through or not
        /// </summary>
        public TBuilder SetPickingMode(PickingMode mode);
        
        
        /// <summary>
        /// Adds a USS class name to the element.
        /// </summary>
        TBuilder AddClass(string className);

        public TBuilder AddClasses(string[] classes);
        
        /// <summary>
        /// Removes a USS class name to the element.
        /// </summary>
        TBuilder RemoveClass(string className);

        /// <summary>
        /// Attaches the element to a parent.
        /// </summary>
        TBuilder AttachTo(VisualElement parent);
        
        /// <summary>
        /// Adds a single child element to this builder's root.
        /// </summary>
        TBuilder AddChild(VisualElement child);
        
         /// <summary>
        /// Adds a single child built from another builder.
        /// </summary>
        TBuilder AddChild<TChildElement, TChildBuilder>(IUIBuilder<TChildElement, TChildBuilder> childBuilder)
            where TChildElement : VisualElement
            where TChildBuilder : IUIBuilder<TChildElement, TChildBuilder>;
         
         
        TBuilder AddChildren<TChildElement, TChildBuilder>(IEnumerable<IUIBuilder<TChildElement, TChildBuilder>> childBuilders)
            where TChildElement : VisualElement
            where TChildBuilder : IUIBuilder<TChildElement, TChildBuilder>;

        
        
        //Common config methods
        TBuilder SetPadding(float left, float right, float top, float bottom);
        
        TBuilder SetMinMax(float minWidth, float minHeight, float maxWidth, float maxHeight);
        
        TBuilder SetFixedSize(float width, float height);
        
        TBuilder SetFlexDirection(FlexDirection direction);
        
        TBuilder OnGeometryChanged(EventCallback<GeometryChangedEvent> callback);



        TBuilder SetBorder(float border);
        TBuilder SetBorderColor( Color color );
        TBuilder SetBorderRadius( float radius );
        
        

        /// <summary>
        /// Adds a style sheet reference to the element.
        /// </summary>
      //  TBuilder AddStyleSheet(StyleSheet styleSheet);

        /// <summary>
        /// Builds and returns the underlying UI element.
        /// </summary>
        TElement Build();
    }
}
