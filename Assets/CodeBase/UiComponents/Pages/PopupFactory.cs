using System;
using CodeBase.Documents.Neureka.Components;
using ToolBox.Services.Haptics;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Pages
{
    public static class PopupFactory
    {
        public static VisualElement CreateConfirmationPopup(VisualElement root, string title, string message, Action onConfirm)
        {
            return new PopUpBuilder()
                .SetTitleText(title)
                .SetContentText(message)
                .SetPercentageHeight(40)
                .SetConfirmAction(() =>
                {
                    HapticsHelper.RequestHaptics();
                    onConfirm?.Invoke();
                })
                .AttachTo(root).Build();
           
        }

        public static VisualElement CreateQuitPopup(VisualElement root, string title, string message, Action onConfirm, Action onCancel)
        {
            return new PopUpBuilder()
                .SetTitleText(title)
                .SetContentText(message)
                .SetPercentageHeight(60)
                .SetImage("Sprites/panicked_scientist")
                .SetConfirmAction(() =>
                {
                    HapticsHelper.RequestHaptics();
                    onConfirm?.Invoke();
                })
                .SetCancelAction(() =>
                {
                    HapticsHelper.RequestHaptics();
                    onCancel?.Invoke();
                })
                .AttachTo(root)
                .Build();
            
        }

        // Add more popup types as needed...
    }
}