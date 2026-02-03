using System;
using CodeBase.UiComponents.Factories;
using CodeBase.UiComponents.Styles;
using FluentUI.Components;
using UiFrameWork.Components;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.UiComponents.Page
{
    public class SingleButtonFooter
    {

        private readonly VisualElement _visualElement;
        
        private readonly Action _buttonAction;
        private readonly string _buttonText;
        private readonly VisualElement _parent;
        
       // private readonly string _defaultContainerStyleClass = "questionnaire-footer";
       // private readonly string _defaultButtonStyleClass = "btn-large";
        private const string DefaultButtonText = "Confirm";
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


            ButtonFactory.CreateButton(ButtonType.Confirm, DefaultButtonText, _buttonAction, container );
            
            return container;
        }
        
        public static implicit operator VisualElement(SingleButtonFooter footer) => footer._visualElement;
    }
}
