using System;
using UiFrameWork.Builders;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.UiComponents.Page
{
    
    /// <summary>
    /// Standard builder for creating UI page images.
    /// Resolves texture from Texture2D, Sprite, or resource path and applies width/height.
    /// Slightly over engineered but the ends justify the means
    /// </summary>
    
    public class StandardImageBuilder
    {
        private Texture2D _texture;
        private Sprite _sprite;
        private string _resourcePath;
        private int _width;
        private int _height;
        private VisualElement _parent;
        private string[] _classes;
        
        private ScaleMode _scaleMode;
   

        public StandardImageBuilder SetTexture(Texture2D texture)
        {
            _texture = texture;
            return this;
        }

        public StandardImageBuilder SetSprite(Sprite sprite)
        {
            _sprite = sprite;
            return this;
        }

        public StandardImageBuilder SetResourcePath(string path)
        {
            _resourcePath = path;
            return this;
        }

        public StandardImageBuilder SetWidth(int width)
        {
            _width = width;
            return this;
        }

        public StandardImageBuilder SetHeight(int height)
        {
            _height = height;
            return this;
        }

        public StandardImageBuilder SetScaleMode(ScaleMode scaleMode)
        {
            _scaleMode = scaleMode;
            return this;
        }

        public StandardImageBuilder AttachTo(VisualElement parent)
        {
            _parent = parent;
            return this;
        }
        
        public StandardImageBuilder AddClass( string className )
        {
            _classes = new[] { className };
            return this;
        }

        public StandardImageBuilder AddClass( string[] classes )
        {
            _classes = classes;
            return this;
        }

        public Image Build()
        {
            if (_parent == null)
            {
                Logger.LogError( $"parent is null in StandardImageBuilder" );
                return null;
            }
            
            Texture2D texture = ResolveTexture();
            
            Logger.Log( $"Resource path: {_resourcePath}" );
            
            if (texture == null)
            {
                Logger.Log($"texture is null in StandardImageBuilder");
                return null;
            }

            int width = _width != 0 ? _width : texture.width;
            int height = _height != 0 ? _height : texture.height;

            Logger.Log($"Displaying image {_resourcePath ?? "<passed texture/sprite>"} at {width}x{height}");

            return new ImageBuilder()
                .SetTexture(texture)
                .SetWidth(width)
                .SetHeight(height)
                .SetScaleMode(_scaleMode)
                .AttachTo(_parent)
                .AddClasses(_classes ?? Array.Empty<string>())
                .Build();
        }

        private Texture2D ResolveTexture()
        {
            return _texture 
                   ?? _sprite?.texture 
                   ?? (!string.IsNullOrEmpty(_resourcePath) ? Resources.Load<Texture2D>(_resourcePath) : null);
        }
    }
}