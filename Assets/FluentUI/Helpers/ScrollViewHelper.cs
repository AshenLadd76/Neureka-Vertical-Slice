using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluentUI.Helpers
{
    public static class ScrollViewHelper
    {
    
        /// <summary>
        /// Jumps the ScrollView so that the target element is aligned at the top of the viewport.
        /// </summary>
        // public static void JumpToElement(ScrollView scrollView, VisualElement element)
        // {
        //     if (scrollView == null || element == null) return;
        //
        //     // Get element's position relative to scroll content
        //     float elementTop = element.resolvedStyle.top;
        //     
        //     // Clamp to max scrollable range
        //     float maxY = scrollView.contentContainer.layout.height - scrollView.contentViewport.layout.height;
        //     float targetY = Mathf.Clamp(elementTop, 0, Mathf.Max(0, maxY));
        //
        //     // Apply scroll offset
        //     scrollView.scrollOffset = new Vector2(scrollView.scrollOffset.x, targetY);
        // }
        
        public static void JumpToElement(ScrollView scrollView, VisualElement element)
        {
            if (scrollView == null || element == null) return;

            // Height of the viewport
            float viewportHeight = scrollView.contentViewport.resolvedStyle.height;

            // Position of the element relative to scroll content
            float elementTop = element.resolvedStyle.top;

            // Center the element in the viewport
            float targetY = elementTop + element.resolvedStyle.height / 2 - viewportHeight / 2;

            // Clamp to max scrollable range
            float maxY = scrollView.contentContainer.layout.height - viewportHeight;
            targetY = Mathf.Clamp(targetY, 0, Mathf.Max(0, maxY));

            // Apply scroll offset
            scrollView.scrollOffset = new Vector2(scrollView.scrollOffset.x, targetY);
        }
        
        public static void JumpToElementSmooth(ScrollView scrollView, VisualElement element, float duration = 0.3f, long delay = 100)
        {
            const int topOffset = 100;
            
            if (scrollView == null || element == null) return;

            float elementTop = element.resolvedStyle.top - topOffset;
            float maxY = scrollView.contentContainer.layout.height - scrollView.contentViewport.layout.height;
            float targetY = Mathf.Clamp(elementTop, 0, Mathf.Max(0, maxY));

            Vector2 startOffset = scrollView.scrollOffset;
            float elapsed = 0f;

            Action repeat = null;
            repeat = () =>
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                scrollView.scrollOffset = Vector2.Lerp(startOffset, new Vector2(startOffset.x, targetY), t);

                if (t < 1f)
                {
                    // Schedule next frame
                    scrollView.schedule.Execute(repeat).ExecuteLater(0);
                }
            };

            scrollView.schedule.Execute(repeat).ExecuteLater(delay);
        }

        
        public static void JumpTo(ScrollView scrollView, float verticalOffset)
        {
            var offset = scrollView.scrollOffset;
            offset.y = verticalOffset;
            scrollView.scrollOffset = offset;
        }

     
        
    }
}