using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class SliderBuilder : BaseBuilder<Slider, SliderBuilder>
    {
        private float _minValue;
        private float _maxValue;
        
        private float _currentValue;

        private float _step;
        
        private Action<float> _onValueChanged;


        public SliderBuilder SetLabelText( string labelText )
        {
            _visualElement.label = labelText;
            return this;
        }
        

        public SliderBuilder SetDirection(SliderDirection direction)
        {
            _visualElement.direction = direction;
            return this;
        }
        

        public SliderBuilder SetMinValue(float minValue)
        {
            _visualElement.lowValue = minValue;
            return this;
        }

        public SliderBuilder SetMaxValue(float maxValue)
        {
            _visualElement.highValue = maxValue;
            return this;
        }

        public SliderBuilder SetCurrentValue(float currentValue)
        {
            _visualElement.value = Mathf.Clamp(currentValue, _minValue, _maxValue);
            return this;
        }

        public SliderBuilder SetStep(float step)
        {
            _step = step;
            return this;
        }

        public SliderBuilder Invert(bool isInverted)
        {
            _visualElement.inverted = isInverted;
            
            return this;
        }

        public SliderBuilder Visible(bool isVisible)
        {
            _visualElement.visible = isVisible;
            return this;
        }
        
        public SliderBuilder OnValueChanged(Action<float> callback)
        {
            _onValueChanged  = callback ?? throw new ArgumentNullException(nameof(callback));

            _visualElement.RegisterValueChangedCallback(evt =>
            {
                float snappedValue = Mathf.Round( evt.newValue / _step ) * _step;
                _visualElement.value = snappedValue;
                
                _onValueChanged?.Invoke(snappedValue);
            });
            
            return this;
        }
    }
}
