using UnityEngine;

namespace UiFrameWork.Helpers
{
    public static class TexturePatterns
    {
        public static Color[] GenerateCheckerPattern(int width, int height, int squareSize, Color colorA, Color colorB)
        {
            Color[] pixels = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool isEven = ((x / squareSize) + (y / squareSize)) % 2 == 0;
                    pixels[y * width + x] = isEven ? colorA : colorB;
                }
            }

            return pixels;
        }
        
        public static Texture2D CreateTexture(int width, int height, int tileWidth, int tileHeight)
        {
            Texture2D tex = new Texture2D(width, height);
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color c = ((x / tileWidth + y / tileHeight) % 2 == 0) ? Color.gray : Color.white;
                    tex.SetPixel(x, y, c);
                }
            }

            tex.Apply();
            return tex;
        }
    }
}