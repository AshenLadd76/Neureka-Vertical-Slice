using System;
using UiFrameWork.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.Documents.Neureka.Navigation
{
    public class InfoBoxBuilder
    {
        private string _infoText;
        private Action _onClick;
        private VisualElement _parent;

        private const int AnimationDelayMs = 300;
        
        public InfoBoxBuilder SetText(string text)
        {
            _infoText = text;
            return this;
        }

        public InfoBoxBuilder SetAction(Action onclick)
        {
            _onClick = onclick;
            return this;
        }

        public InfoBoxBuilder AttachTo(VisualElement parent)
        {
            _parent = parent;
            return this;
        }
        
        
        public void Build()
        {
            var infoBoxContainer = new ContainerBuilder().AddClass("info-box").AttachTo(_parent).Build();
            
            var infoBoxText = new LabelBuilder().SetText(_infoText).AddClass("info-box-label").AttachTo(infoBoxContainer).Build();
            
            var closeButton = new ContainerBuilder().AddClass("info-box-close-button").AttachTo(infoBoxContainer).OnClick(() =>
            {
                _onClick?.Invoke();
                
                infoBoxContainer.style.opacity = 1f;
                infoBoxContainer.style.scale = new Scale(new Vector3(1f, 1f, 1f));
                
                // Wait one frame so UITK resolves the style
                infoBoxContainer.schedule.Execute(() =>
                {
                    infoBoxContainer.style.opacity = 0f;
                    infoBoxContainer.style.scale = new Scale(new Vector3(1f, 0f, 1f));
                    
                    infoBoxContainer.schedule.Execute(() =>
                    {
                        infoBoxContainer.style.display = DisplayStyle.None;
                    }).StartingIn(AnimationDelayMs); // match transition duration
                    
                }).StartingIn(0);
                
            }).Build();
        }
    }
}