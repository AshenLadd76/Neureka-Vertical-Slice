using System;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Headers
{
    public abstract class BaseUiComponent<T> where T : VisualElement
    {
        protected readonly T VisualElement;

        protected BaseUiComponent(T element)
        {
            VisualElement = element ?? throw new ArgumentNullException(nameof(element));
        }

        public static implicit operator VisualElement(BaseUiComponent<T> component) => component.VisualElement;
    }
}