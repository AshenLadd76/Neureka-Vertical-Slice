using UnityEngine;
using UnityEngine.UIElements;

namespace ToolBox.Utils
{
    public static class UIShadowHelper
    {
        /// <summary>
        /// Adds a drop shadow behind a VisualElement.
        /// </summary>
        /// <param name="target">The element to shadow.</param>
        /// <param name="color">Shadow color (default semi-transparent black).</param>
        /// <param name="offsetX">Horizontal offset in pixels.</param>
        /// <param name="offsetY">Vertical offset in pixels.</param>
        /// <param name="blur">Extra padding for a soft shadow (simulated by size increase).</param>
        public static void AddDropShadow(VisualElement target, Color? color = null, float offsetX = 2f, float offsetY = 2f, float blur = 4f)
        {
            if (target == null || target.parent == null)
                return;

            Color shadowColor = color ?? new Color(0, 0, 0, 0.3f);

            var shadow = new VisualElement
            {
                name = "shadow"
            };

            shadow.style.position = Position.Absolute;
            shadow.style.backgroundColor = shadowColor;

            // Match target size + extra for "blur"
            shadow.style.width = target.resolvedStyle.width + blur;
            shadow.style.height = target.resolvedStyle.height + blur;

            // Offset the shadow
            shadow.style.left = target.resolvedStyle.left + offsetX - blur / 2f;
            shadow.style.top = target.resolvedStyle.top + offsetY - blur / 2f;

            // Make sure it renders behind
            target.parent.Insert(0, shadow);
        }
    }
}