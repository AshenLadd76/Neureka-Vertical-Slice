using System;
using System.Collections.Generic;
using UiFrameWork.Builders;
using UnityEngine.UIElements;

namespace UiFrameWork.Components
{
    public class ChoiceGroupBuilder : BaseBuilder<VisualElement, ChoiceGroupBuilder>
    {
        private readonly List<VisualElement> _options = new();
        private bool _allowMultipleSelection = false;
        private int? _selectedIndex = null;
        
        public ChoiceGroupBuilder AllowMultipleSelection(bool allowMultipleSelection)
        {
            _allowMultipleSelection = allowMultipleSelection;
            return this;
        }

        public ChoiceGroupBuilder AddOption(string optionText, Action<int, bool> onSelectionChanged)
        {
            int index = _options.Count;
            
            var checkbox = new ButtonBuilder().SetText(optionText).AddClass("option-button").AttachTo(VisualElement)
                .OnClick(()=>
                { 
                    if( !_allowMultipleSelection ) ResetOptions();
                    
                    bool isSelected = _options[index].ClassListContains("option-button-selected");
                    
                    onSelectionChanged?.Invoke(index, !isSelected);
                    ToggleSelected(isSelected, index);
                })
                .Build();
            
            _options.Add(checkbox);

            return this;
        }

        

     

        private void ToggleSelected(bool selected, int index)
        {
            var selectedCheckBox = _options[index];

            if (!selected)
                selectedCheckBox.AddToClassList("option-button-selected");
            else
                selectedCheckBox.RemoveFromClassList("option-button-selected");
        }

        private void ResetOptions()
        {
            foreach (var t in _options)
                t.RemoveFromClassList("option-button-selected");
        }
    }
}
