using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluentUI.Components
{
    public class SliderBuilder : Builders.BaseBuilder<Slider, SliderBuilder>
    {
        private float _minValue;
        private float _maxValue;
        
        private float _currentValue;

        private float _step;
        
        private Action<float> _onValueChanged;


        public SliderBuilder SetLabelText( string labelText )
        {
            VisualElement.label = labelText;
            return this;
        }
        

        public SliderBuilder SetDirection(SliderDirection direction)
        {
            VisualElement.direction = direction;
            return this;
        }
        

        public SliderBuilder SetMinValue(float minValue)
        {
            VisualElement.lowValue = minValue;
            return this;
        }

        public SliderBuilder SetMaxValue(float maxValue)
        {
            VisualElement.highValue = maxValue;
            return this;
        }

        public SliderBuilder SetCurrentValue(float currentValue)
        {
            VisualElement.value = Mathf.Clamp(currentValue, _minValue, _maxValue);
            return this;
        }

        public SliderBuilder SetStep(float step)
        {
            _step = step;
            return this;
        }

        public SliderBuilder Invert(bool isInverted)
        {
            VisualElement.inverted = isInverted;
            
            return this;
        }

        public SliderBuilder Visible(bool isVisible)
        {
            VisualElement.visible = isVisible;
            return this;
        }
        
        public SliderBuilder OnValueChanged(Action<float> callback)
        {
            _onValueChanged  = callback ?? throw new ArgumentNullException(nameof(callback));

            VisualElement.RegisterValueChangedCallback(evt =>
            {
                float snappedValue = Mathf.Round( evt.newValue / _step ) * _step;
                VisualElement.value = snappedValue;
                
                _onValueChanged?.Invoke(snappedValue);
            });
            
            return this;
        }
    }
}
