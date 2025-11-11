using System;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.Documents.DemoA.Components
{
    public class ImageCard
    {
        
        public ImageCard(VisualElement parent, Action headerButton1Action, Action headerButton2Action, string blurbText = null)
        {
            if( parent == null ) throw new ArgumentNullException(nameof(parent));
            if( headerButton1Action == null ) throw new ArgumentNullException(nameof(headerButton1Action));
            if( headerButton2Action == null ) throw new ArgumentNullException(nameof(headerButton2Action));
            
             
            Build(parent, headerButton1Action, headerButton2Action, blurbText);
        }

        private void Build(VisualElement parent, Action headerButton1Action, Action headerButton2Action, string blurbText = null)
        {
            var shadowContainer = new ContainerBuilder().AddClass(UiStyleClassDefinitions.ImageShadowContainer).AttachTo(parent).Build();
            
            var imageContainer = new ContainerBuilder().AddClass(UiStyleClassDefinitions.ImageContainer).AttachTo(shadowContainer).Build();
            
            var imageHeader = new ContainerBuilder().AddClass(UiStyleClassDefinitions.InfoImageHeader).AttachTo(imageContainer).Build();
            
            new ButtonBuilder().AddClass(UiStyleClassDefinitions.ImageHeaderButton).OnClick(headerButton1Action).AttachTo(imageHeader).Build();
            
            new ButtonBuilder().AddClass(UiStyleClassDefinitions.ImageHeaderButton).OnClick(headerButton2Action).AttachTo(imageHeader).Build();
            
            new ContainerBuilder().AddClass(UiStyleClassDefinitions.InfoImageBlurb).AttachTo(imageContainer).Build();
        }
        
    }
}
