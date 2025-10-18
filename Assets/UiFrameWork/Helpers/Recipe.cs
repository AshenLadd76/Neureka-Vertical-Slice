using System;
using UnityEngine.UIElements;

namespace UiFrameWork.Helpers
{
    public class Recipe<T>
    {
        public string ID { get; }
        public Func<VisualElement, T> Builder { get; }

        public Recipe(string id, Func<VisualElement, T> builder)
        {
            ID = id;
            Builder = builder;
        }
    }
}