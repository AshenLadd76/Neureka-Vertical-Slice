using System;
using CodeBase.Documents.DemoA.Pages;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine;

using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.DemoA.Components
{
    public class MenuCardBuilder
    {
        private VisualElement _parent;
        private string _title = "Menu Card Title";
        private string _blurb = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
        private Sprite _icon;
        private float _progress = 0f;

        private Action _onClick;

        // Optional: colors or other styling can be added as needed
        private Color _iconBackgroundColor = ColorUtils.GetRandomColor();

        public MenuCardBuilder SetParent(VisualElement parent)
        {
            _parent = parent;
            return this;
        }

        public MenuCardBuilder SetTitle(string title)
        {
            _title = title;
            return this;
        }

        public MenuCardBuilder SetBlurb(string blurb)
        {
            _blurb = blurb;
            return this;
        }

        public MenuCardBuilder SetIcon(Sprite icon)
        {
            _icon = icon;
            return this;
        }

        public MenuCardBuilder SetProgress(float progress)
        {
            _progress = Mathf.Clamp01(progress);
            return this;
        }

        public MenuCardBuilder SetIconBackgroundColor(Color color)
        {
            _iconBackgroundColor = color;
            return this;
        }

        public MenuCardBuilder SetAction(Action action)
        {
            _onClick = action;
            return this;
        }

        public VisualElement Build()
        {
            if (_parent == null)
                throw new System.Exception("MenuCardBuilder: Parent must be set.");

            // Outer container
            var outerContainer = new ContainerBuilder()
                .AddClass(UssClassNames.MenuCard)
                .AttachTo(_parent)
                .OnClick(_onClick)
                .Build();

            // Left side: icon container
            var menuCardIconContainer = new ContainerBuilder()
                .AddClass(UssClassNames.MenuCardIconContainer)
                .SetBackgroundColor(_iconBackgroundColor)
                .AttachTo(outerContainer)
                .Build();

            
            new ImageBuilder()
                .SetSprite(_icon)
                .AddClass(UssClassNames.MenuCardIcon)
                .AttachTo(menuCardIconContainer)
                .Build();
            

            // Right side: text container
            var menuTextContainer = new ContainerBuilder()
                .AddClass(UssClassNames.MenuCardContentContainer)
                .AttachTo(outerContainer)
                .Build();

            new LabelBuilder()
                .SetText(_title)
                .AddClass(UssClassNames.MenuCardTitle)
                .AttachTo(menuTextContainer)
                .Build();

            new LabelBuilder()
                .SetText(_blurb)
                .AddClass(UssClassNames.MenuCardBlurb)
                .AttachTo(menuTextContainer)
                .Build();

            // Progress bar
            var progressBarContainer = new ContainerBuilder()
                .AddClass(UssClassNames.MenuCardProgressBarContainer)
                .AttachTo(menuTextContainer)
                .Build();

            // new ProgressBarBuilder()
            //     .SetWidthPercent(100)
            //     .SetHeight(50)
            //     .SetBackgroundColor(Color.green)
            //     .SetMaxFill(1f)
            //     .SetFillAmount(_progress)
            //     .AttachTo(progressBarContainer)
            //     .Build();

            return outerContainer;
        }
    }
}
