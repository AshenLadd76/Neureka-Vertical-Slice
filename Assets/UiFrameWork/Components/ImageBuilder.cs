using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace UiFrameWork.Builders
{
    public class ImageBuilder : BaseBuilder<Image, ImageBuilder>
    {
        private Texture2D _texture;
        
        public ImageBuilder SetSprite(Sprite sprite)
        {
            if (sprite == null)
            {
                Logger.LogError( $"Supplied Sprite is null" );
                return this;
            }
            
            Logger.Log( $"Sprite is aok !!!!!" );

            VisualElement.sprite = sprite;
            return this;
        }

        public ImageBuilder SetTexture(Texture2D texture)
        {
            if (texture == null)
            {
                Logger.LogError( $"Supplied Texture is null" );
                return this;
            }

            _texture = texture;
            VisualElement.image = texture;
            
            return this;
        }

        public ImageBuilder SetPixels(Color[] pixels, int width, int height)
        {
            if (pixels == null || pixels.Length == 0) return this;

            var tex = new Texture2D(width, height);
            tex.SetPixels(pixels);
            tex.Apply();
            VisualElement.image = tex;

            return this;
        }

        public ImageBuilder SetScaleMode(ScaleMode mode)
        {
            VisualElement.scaleMode = mode;
            return this;
        }
        
        public ImageBuilder UseNativeSize()
        {
            if (_texture != null)
            {
                VisualElement.style.width = _texture.width;
                VisualElement.style.height = _texture.height;
            }
            return this;
        }

        public ImageBuilder SetTint(Color color)
        {
            VisualElement.tintColor = color;
            return this;
        }
        
        public ImageBuilder SetNineSlice(RectOffset border)
        {
            return this;
        }

        public ImageBuilder FlipVertical()
        {
            VisualElement.transform.scale = new Vector3(1, -1, 1);
            return this;
        }

        public ImageBuilder FlipHorizontal()
        {
            VisualElement.transform.scale = new Vector3(-1, 1, 1);
            return this;
        }
    }
}
