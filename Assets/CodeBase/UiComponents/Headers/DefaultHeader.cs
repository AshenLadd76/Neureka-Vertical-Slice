using System;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Headers
{
    public class DefaultHeader : BaseUiComponent<VisualElement>
    {
        public DefaultHeader(string headerText, VisualElement parent, Action onBack = null, Action onClose = null) 
            : base(Build(headerText, parent,  onBack, onClose)) { }

        private static VisualElement Build(string headerText, VisualElement parent = null, Action onBack = null, Action onClose = null)
        {
            VisualElement container =  new ContainerBuilder()
                .AddClass( UiStyleClassDefinitions.Header )
                .AttachTo(parent)
                .Build();
            
            if (onBack != null)
            {
                new ButtonBuilder().SetText("Back")
                    .OnClick(onBack)
                    .AddClass(UiStyleClassDefinitions.HeaderButton)
                    .AttachTo(container)
                    .Build();
            }
            
            new LabelBuilder().SetText(headerText)
                .AttachTo(container)
                .Build();
            
            if (onClose != null)
            {
                new ButtonBuilder().SetText("Close")
                    .OnClick(onClose)
                    .AddClass(UiStyleClassDefinitions.HeaderButton)
                    .AddClass("Label")
                    .AttachTo(container)
                    .Build();
            }
            
            return container;
        }
        
        public static implicit operator VisualElement(DefaultHeader header) => header.VisualElement;
    }
} 
     
    

