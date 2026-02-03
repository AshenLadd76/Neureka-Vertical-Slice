using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
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

        public TBuilder AttachTo(VisualElement parent);
        

        /// <summary>
        /// Adds a style sheet reference to the element.
        /// </summary>
        /// TBuilder AddStyleSheet(StyleSheet styleSheet);

        /// <summary>
        /// Builds and returns the underlying UI element.
        /// </summary>
        TElement Build();
    }
}
