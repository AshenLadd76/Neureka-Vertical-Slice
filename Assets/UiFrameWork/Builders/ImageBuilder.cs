using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class ImageBuilder : BaseBuilder<Image, ImageBuilder>
    {
        public ImageBuilder SetSprite(Sprite sprite)
        {
            if (sprite == null) return this;
            _visualElement.sprite = sprite;
            return this;
        }

        public ImageBuilder SetTexture(Texture2D texture)
        {
            if (texture == null) return this;
            _visualElement.image = texture;
            return this;
        }

        public ImageBuilder SetPixels(Color[] pixels, int width, int height)
        {
            if (pixels == null || pixels.Length == 0) return this;

            var tex = new Texture2D(width, height);
            tex.SetPixels(pixels);
            tex.Apply();
            _visualElement.image = tex;

            return this;
        }

        public ImageBuilder SetScaleMode(ScaleMode mode)
        {
            _visualElement.scaleMode = mode;
            return this;
        }

        public ImageBuilder SetTent(Color color)
        {
            _visualElement.tintColor = color;
            return this;
        }
        
        public ImageBuilder SetNineSlice(RectOffset border)
        {
            return this;
        }

        public ImageBuilder FlipVertical()
        {
            _visualElement.transform.scale = new Vector3(1, -1, 1);
            return this;
        }

        public ImageBuilder FlipHorizontal()
        {
            _visualElement.transform.scale = new Vector3(-1, 1, 1);
            return this;
        }
    }
}
