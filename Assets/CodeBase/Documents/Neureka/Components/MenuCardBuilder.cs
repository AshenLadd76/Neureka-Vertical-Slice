using System;
using CodeBase.Documents.DemoA;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Components
{
    public class MenuCardBuilder
    {
        private VisualElement _parent;
        private string _title = "Menu Card Title";
        private string _blurb = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
        private Sprite _icon;
        private float _progress = 0f;
        
        private const float DragThreshold = 5f;

        private Action _onClick;

        // Optional: colors or other styling can be added as needed
        private Color _iconBackgroundColor;

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

            var menuCardParent = new ContainerBuilder().AttachTo(_parent).Build();
            
           // var shadow =  new ContainerBuilder().AddClass("menu-card-shadow").AttachTo(menuCardParent).Build();
            
            // Outer container
            var menuCard = new ContainerBuilder()
                .AddClass(UssClassNames.MenuCard)
                .AttachTo(menuCardParent)
                //.OnClick(_onClick)
                .Build();
            
            

            Vector3 startPos = Vector3.zero;
            bool isDragging = false;
            
            menuCard.RegisterCallback<PointerDownEvent>(evt =>
            {
                // record the pointer start position
                startPos = evt.position;
                isDragging = false;
                
                Logger.Log( $"On Pointer down event: {evt.position}" );
                //menuCard.CapturePointer(evt.pointerId);
                
            });

            menuCard.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (Vector2.Distance(evt.position, startPos) > DragThreshold)
                    isDragging = true;
                
            });

            menuCard.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (!isDragging)
                    _onClick?.Invoke();
            });
            
            menuCard.RegisterCallback<PointerCaptureOutEvent>(evt =>
            {
                menuCard.ReleasePointer(evt.pointerId);
                
                isDragging = false;
            });

            
            
            // Left side: icon container
            var menuCardIconContainer = new ContainerBuilder()
                .AddClass(UssClassNames.MenuCardIconContainer)
                .SetBackgroundColor(_iconBackgroundColor)
                .AttachTo(menuCard)
                .Build();


            if (_icon == null)
            {
                Logger.Log( $"Menu card icon is null." );
            }
            
            var menuCardIconBackground = new ContainerBuilder().AddClass(UssClassNames.MenuIconBackground).AttachTo(menuCardIconContainer).Build();
            
            new ImageBuilder()
                .SetSprite(_icon)
                .AddClass(UssClassNames.MenuCardIcon)
                .AttachTo(menuCardIconBackground)
                .Build();
            

            // Right side: text container
            var menuTextContainer = new ContainerBuilder()
                .AddClass(UssClassNames.MenuCardContentContainer)
                .AttachTo(menuCard)
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
            //     .SetFillClass(UssClassNames.MenuCardProgressBar)
            //     .SetWidthPercent(100)
            //     .SetHeightPercent(20)
            //     .SetMaxFill(1f)
            //     .SetFillAmount(_progress)
            //     .AttachTo(progressBarContainer)
            //     .Build();

            return menuCard;
        }
        
        private void BuildProgressBar(VisualElement parent)
        {
           // new ProgressBarBuilder().SetWidthPercent(100).SetWidthPercent(25).SetFillClass(UssClassNames.MenuCardProgressBar).SetBackgroundColor(Color.green).SetMaxFill(1f).SetFillAmount(1f).AttachTo(parent).Build();
        }
    }
}
