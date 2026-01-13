using UnityEngine;

public static class UiColourHelper
{
    public static Color GetRandomAccentColor()
    {
        return Color.HSVToRGB(
            Random.value, // Hue
            0.5f,         // Saturation
            0.9f          // Value
        );
    }
}

