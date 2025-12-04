using System;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Headers
{
    public class DefaultHeader : BaseUiComponent<VisualElement>
    {
        public DefaultHeader(string headerText, VisualElement parent, Action onBack = null, Action onClose = null,   string headerUssClass = null, string buttonUssClass = null, string labelUssClass = null) 
            : base(Build(headerText, parent,  onBack, onClose, headerUssClass, buttonUssClass, labelUssClass)) { }

        private static VisualElement Build(string headerText, VisualElement parent = null, Action onBack = null, Action onClose = null, string headerUssClass = null, string buttonUssClass = null, string labelUssClass = null)
        {
            // Use defaults if null
            headerUssClass ??= UiStyleClassDefinitions.Header;
            buttonUssClass ??= UiStyleClassDefinitions.HeaderButton;
            labelUssClass ??= UiStyleClassDefinitions.HeaderLabel; // you can define this in your USS definitions

            
            VisualElement container =  new ContainerBuilder()
                .AddClass( headerUssClass )
                .AttachTo(parent)
                .Build();
            
            // if (onBack != null)
            // {
            //     new ButtonBuilder().SetText("Back")
            //         .OnClick(onBack)
            //         .AddClass(buttonUssClass)
            //         .AttachTo(container)
            //         .Build();
            // }
            
            new LabelBuilder().SetText(headerText)
                .AttachTo(container)
                .AddClass(labelUssClass)
                .Build();
            
            if (onClose != null)
            {
                new ButtonBuilder().SetText("X")
                    .OnClick(onClose)
                    .AddClass(buttonUssClass)
                    .AddClass(labelUssClass)
                    .AttachTo(container)
                    .Build();
            }
            
            return container;
        }
        
        public static implicit operator VisualElement(DefaultHeader header) => header.VisualElement;
    }
} 
     
    

