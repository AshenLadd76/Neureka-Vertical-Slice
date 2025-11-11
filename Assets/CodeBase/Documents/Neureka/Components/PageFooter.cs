using System;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.Documents.DemoA.Components
{
    public class PageFooter 
    {
        public PageFooter( VisualElement parent, string buttonText, Action onClick, string style = UiStyleClassDefinitions.SharedFooter )
        {
            if(parent == null) throw new ArgumentNullException(nameof(parent));
            if(string.IsNullOrEmpty(buttonText)) throw new ArgumentException(nameof(buttonText));
            if(onClick == null) throw new ArgumentNullException(nameof(onClick));
            
            Build( parent, buttonText, onClick, style );
        }
        
        private void Build(VisualElement parent, string buttonText, Action action, string style )
        {
            var footer =  new ContainerBuilder().AddClass(style).AttachTo(parent).Build();

            new PrimaryButton( footer, buttonText, action );
        }
    }
}
