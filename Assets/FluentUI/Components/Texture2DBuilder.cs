using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class Texture2DBuilder 
    {
        private int _width = 128;
        private int _height = 128;
        private TextureFormat _format = TextureFormat.RGBA32;
        private bool _mipChain = false;
        private bool _linear = false;
        private FilterMode _filterMode = FilterMode.Point;
        private TextureWrapMode _wrapMode = TextureWrapMode.Clamp;
        private Color[] _pixels;
        private float _pixelsPerUnit = 100f;
        private VisualElement _parent;
        
        // width & height
        public Texture2DBuilder SetWidth(int width) { _width = width; return this; }
        public Texture2DBuilder SetHeight(int height) { _height = height; return this; }
        
        // Set pixel data
        public Texture2DBuilder SetPixels(Color[] pixels) { _pixels = pixels; return this; }

        
        // Format & mipmaps
        public Texture2DBuilder SetFormat(TextureFormat format) { _format = format; return this; }
        public Texture2DBuilder EnableMipMap(bool enabled = true) { _mipChain = enabled; return this; }
        public Texture2DBuilder UseLinearColorSpace(bool linear = true) { _linear = linear; return this; }
        
        // Filter & wrap
        public Texture2DBuilder SetFilterMode(FilterMode mode) { _filterMode = mode; return this; }
        public Texture2DBuilder SetWrapMode(TextureWrapMode mode) { _wrapMode = mode; return this; }
        
        // PPU setter
        public Texture2DBuilder SetPixelsPerUnit(float ppu) { _pixelsPerUnit = Mathf.Max(1f, ppu); return this; }
        
        // AttachTo pattern
        public Texture2DBuilder AttachTo(VisualElement parent) 
        { 
            _parent = parent; 
            return this; 
        }
        

        public Texture2D BuildTexture()
        {
            var texture = new Texture2D(_width, _height, _format, _mipChain, _linear)
            {
                
                filterMode = _filterMode,
                wrapMode = _wrapMode,
            };
            
            if( _pixels != null && _pixels.Length == _width * _height )
                texture.SetPixels(_pixels);
            
            texture.Apply();
            
            return texture;
        }
        
        // Optional: build a Sprite from this texture
        public Sprite BuildSprite()
        {
            Texture2D tex = BuildTexture();
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), _pixelsPerUnit);
        }
        
        public Image BuildImage()
        {
            var image = new Image
            {
                image = BuildTexture(),
                //scaleMode = ScaleMode.ScaleToFit,
                
                style =
                {
                    marginLeft = 0,
                    marginRight = 0,
                    marginTop = 0,
                    marginBottom = 0,
                    
                    paddingLeft = 0,
                    paddingRight = 0,
                    paddingTop = 0,
                    paddingBottom = 0,
                }
            };

            _parent?.Add(image);
            
            return image;
        }
    }
}
