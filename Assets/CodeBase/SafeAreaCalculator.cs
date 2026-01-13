using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase
{
    
    //For ios
    public class SafeAreaCalculator
    {
        public void CalculateSafeArea(VisualElement safeAreaContainer, Rect safeAreaRect)
        {
            if (safeAreaContainer == null)
            {
                Logger.LogError("SafeAreaContainer is null");
                return;
            }
            
            Logger.Log( $"SafeAreaCalculator.CalculateSafeArea({safeAreaRect})" );

            var leftOffset = safeAreaRect.x;
            var bottomOffset = safeAreaRect.y;
            var rightOffset = Screen.width - (safeAreaRect.x + safeAreaRect.width);
            var topOffset = Screen.height - (safeAreaRect.y + safeAreaRect.height);
            
            Logger.Log( $"L: { leftOffset } B: { bottomOffset } R: { rightOffset } T: { topOffset }" );
            
            safeAreaContainer.style.left = leftOffset;
            safeAreaContainer.style.right = rightOffset;
            safeAreaContainer.style.top = topOffset;
            safeAreaContainer.style.bottom = bottomOffset;
        }
    }
}