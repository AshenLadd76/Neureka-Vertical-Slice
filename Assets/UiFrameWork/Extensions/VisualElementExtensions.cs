using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Extensions
{
    public static class VisualElementExtensions
    {
        /// <summary>
        /// Changes the background color of a VisualElement after a delay in seconds.
        /// </summary>
        public static void ChangeColorAfterDelay(this VisualElement element, Color color, long delayMilliSeconds)
        {
            // Schedule a single execution after delaySeconds
            element?.schedule.Execute((e) =>
            {
                element.style.backgroundColor = new StyleColor(color);
            }).ExecuteLater(delayMilliSeconds);
        }
        
        /// <summary>
        /// Performs a ping-pong scale animation on a VisualElement over the given duration.
        /// </summary>
        public static void Bounce(this VisualElement element, float scaleAmount = 1.2f, long durationSeconds = 300)
        {
            if (element == null) return;

            long halfDuration = durationSeconds / 2;
            Scale originalScale = element.style.scale.value;

            // Scale up
            element.schedule.Execute((e) =>
            {
                element.style.scale = new Scale(new Vector3(scaleAmount, scaleAmount, 1));
            }).ExecuteLater(0);

            // Scale back down after half the duration
            element.schedule.Execute((e) =>
            {
                element.style.scale = new Scale(originalScale.value);
            }).ExecuteLater(halfDuration);
        }
        
        public static void BounceSmooth(this VisualElement element, float targetScale = 1.3f, float duration = 0.3f)
        {
            if (element == null) return;

            float halfDuration = duration / 2f;
            Scale originalScale = element.style.scale.value;

            float elapsed = 0f;

            // Schedule repeated execution every frame
            var scheduled = element.schedule.Execute((e) =>
            {
                elapsed += e.deltaTime;

                // scale up for first half, scale down for second half
                float t = Mathf.Clamp01(elapsed / halfDuration);
                if (elapsed <= halfDuration)
                {
                    // ease out for scale up
                    float scale = Mathf.Lerp(originalScale.value.x, targetScale, Mathf.Sin(t * Mathf.PI * 0.5f));
                    element.style.scale = new Scale(new Vector3(scale, scale, 1));
                }
                else
                {
                    // ease in for scale down
                    t = Mathf.Clamp01((elapsed - halfDuration) / halfDuration);
                    float scale = Mathf.Lerp(targetScale, originalScale.value.x, Mathf.Sin(t * Mathf.PI * 0.5f));
                    element.style.scale = new Scale(new Vector3(scale, scale, 1));
                }

                // stop after full duration
                if (elapsed >= duration)
                {
                    element.style.scale = new Scale(originalScale.value);
                    
                }

            }).Every(0); // every frame
        }
        
        /// <summary>
        /// Fades in the element from 0 to target opacity over the given duration.
        /// </summary>
        public static void FadeIn(this VisualElement ve, float targetOpacity = 0.5f, float duration = 0.3f, Action onComplete = null)
        {
            ve.style.opacity = 0f;
            ve.style.display = DisplayStyle.Flex; // make sure it's visible

            float elapsed = 0f;
            float startOpacity = 0f;

            IVisualElementScheduledItem scheduledItem = null;

            scheduledItem = ve.schedule.Execute((sched) =>
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                ve.style.opacity = Mathf.Lerp(startOpacity, targetOpacity, t);

                if (t >= 1f)
                {
                    onComplete?.Invoke();
                }
            }).Every(1); // run every frame
        }
        

    }
}
