using System;
using CodeBase.UiComponents.Factories;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.UiComponents.Footers
{
    public class SingleButtonFooter
    {

        private readonly VisualElement _visualElement;
        
        private readonly Action _buttonAction;
        private readonly string _buttonText;
        private readonly VisualElement _parent;
        
        private readonly string _defaultContainerStyleClass = "single-button-footer";
        private readonly string _defaultButtonStyleClass = "btn-large";
        private readonly string _defaultButtonText = "Click Me";
        private readonly Action _defaultAction = () => {  Logger.Log( $"Hey i need you to add a proper action!" ); }; 

        public SingleButtonFooter(Action buttonAction, string buttonText, VisualElement parent = null)
        {
            _buttonAction = buttonAction ?? throw new ArgumentNullException(nameof(buttonAction), "Button action cannot be null.");
            _buttonText = string.IsNullOrWhiteSpace(buttonText) ? throw new ArgumentException("Button text cannot be null or empty.", nameof(buttonText)) : buttonText;
            _parent = parent;
            
            _visualElement = Build();
        }

        private VisualElement Build()
        {
            VisualElement container =  new ContainerBuilder()
                .AddClass( UiStyleClassDefinitions.Footer )
                .AttachTo( _parent )
                .Build();


            ButtonFactory.CreateButton(ButtonType.Confirm, "Confirm", () => { Logger.Log($" I was clicked button factory works!"); }, container );
            
            return container;
        }
        
        public static implicit operator VisualElement(SingleButtonFooter footer) => footer._visualElement;
    }
}
