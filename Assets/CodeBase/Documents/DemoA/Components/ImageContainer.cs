using System;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.Documents.DemoA.Components
{
    public class ImageContainer
    {
        public VisualElement Container { get; private set; }

        public ImageContainer(VisualElement parent, Texture2D texture, float width = 0, float height = 0, ScaleMode scaleMode = ScaleMode.StretchToFill)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (texture == null) throw new ArgumentNullException(nameof(texture));

            Container = BuildContainer(parent, texture, width, height, scaleMode);
        }

        private VisualElement BuildContainer(VisualElement parent, Texture2D texture, float width, float height, ScaleMode scaleMode)
        {
            var container = new ContainerBuilder()
                .AttachTo(parent)
                .Build();

            var image = new ImageBuilder()
                .SetTexture(texture)
                .AttachTo(container)
                .SetScaleMode(scaleMode)
                .Build();

            if (width > 0) container.style.width = width;
            if (height > 0) container.style.height = height;

            return container;
        }
    }
}

