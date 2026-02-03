using CodeBase.UiComponents.Headers;
using FluentUI.Components;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Containers
{
    public class DefaultContainer : BaseUiComponent<VisualElement>
    {
        public DefaultContainer(VisualElement parent = null) : base(Build(parent)) { }

        private static VisualElement Build(VisualElement parent = null)
        {
            var container = new ContainerBuilder()
                .SetWidthPercent(Length.Percent(100))
                .SetHeightPercent(Length.Percent(80))
                .SetFlexDirection(FlexDirection.Column) // horizontal layout for nav items
                .SetAlignItems(Align.Center)
                .SetBackgroundColor(Color.grey)
                .SetPadding(5, 5, 5, 5)
                .SetMargin(5, 5, 5, 5)
                .AttachTo(parent)
                .Build();

            return container;
        }
    }
}
