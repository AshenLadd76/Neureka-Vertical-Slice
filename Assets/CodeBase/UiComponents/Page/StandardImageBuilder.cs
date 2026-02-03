using System;
using FluentUI.Components;
using UiFrameWork.Components;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.UiComponents.Page
{
    
    /// <summary>
    /// Builder class for creating UI images in Unity UI Toolkit.
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
   

        /// <summary>
        /// Sets the <see cref="Texture2D"/> for this image.
        /// Overrides <see cref="Sprite"/> or <see cref="_resourcePath"/> if set.
        /// </summary>
        /// <param name="texture">The texture to use.</param>
        /// <returns>The current builder instance.</returns>
        public StandardImageBuilder SetTexture(Texture2D texture)
        {
            _texture = texture;
            return this;
        }

        
        /// <summary>
        /// Sets the <see cref="Sprite"/> for this image.
        /// Used only if <see cref="_texture"/> is null.
        /// </summary>
        /// <param name="sprite">The sprite to use.</param>
        /// <returns>The current builder instance.</returns>
        public StandardImageBuilder SetSprite(Sprite sprite)
        {
            _sprite = sprite;
            return this;
        }

        /// <summary>
        /// Sets a resource path to load a <see cref="Texture2D"/> from Resources.
        /// Used only if <see cref="_texture"/> and <see cref="_sprite"/> are null.
        /// </summary>
        /// <param name="path">The resource path.</param>
        /// <returns>The current builder instance.</returns>
        public StandardImageBuilder SetResourcePath(string path)
        {
            _resourcePath = path;
            return this;
        }

        /// <summary>
        /// Sets the width of the image.
        /// Defaults to the texture width if not specified.
        /// </summary>
        /// <param name="width">The desired width in pixels.</param>
        /// <returns>The current builder instance.</returns>
        public StandardImageBuilder SetWidth(int width)
        {
            _width = width;
            return this;
        }

        /// <summary>
        /// Sets the height of the image.
        /// Defaults to the texture height if not specified.
        /// </summary>
        /// <param name="height">The desired height in pixels.</param>
        /// <returns>The current builder instance.</returns>
        public StandardImageBuilder SetHeight(int height)
        {
            _height = height;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="ScaleMode"/> for this image.
        /// Determines how the texture is scaled inside the Image element.
        /// </summary>
        /// <param name="scaleMode">The scale mode to apply.</param>
        /// <returns>The current builder instance.</returns>
        public StandardImageBuilder SetScaleMode(ScaleMode scaleMode)
        {
            _scaleMode = scaleMode;
            return this;
        }

        /// <summary>
        /// Sets the parent <see cref="VisualElement"/> to attach the built image to.
        /// Must be called before <see cref="Build"/> or the builder will log an error.
        /// </summary>
        /// <param name="parent">The parent container element.</param>
        /// <returns>The current builder instance.</returns>
        public StandardImageBuilder AttachTo(VisualElement parent)
        {
            _parent = parent;
            return this;
        }
        
        /// <summary>
        /// Assigns a single USS class to the image.
        /// Overwrites any previously set classes.
        /// </summary>
        /// <param name="className">The class name to assign.</param>
        /// <returns>The current builder instance.</returns>
        public StandardImageBuilder AddClass( string className )
        {
            _classes = new[] { className };
            return this;
        }

        /// <summary>
        /// Assigns multiple USS classes to the image.
        /// Overwrites any previously set classes.
        /// </summary>
        /// <param name="classes">The array of class names to assign.</param>
        /// <returns>The current builder instance.</returns>
        public StandardImageBuilder AddClass( string[] classes )
        {
            _classes = classes;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="Image"/> element with all configured settings.
        /// Resolves the texture from <see cref="_texture"/>, <see cref="_sprite"/>, or <see cref="_resourcePath"/>.
        /// Attaches it to the parent and applies width, height, scale mode, and classes.
        /// Logs errors if the parent is null or the texture cannot be resolved.
        /// </summary>
        /// <returns>The constructed <see cref="Image"/> element, or null if creation failed.</returns>

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
            
            return new ImageBuilder()
                .SetTexture(texture)
                .SetWidth(width)
                .SetHeight(height)
                .SetScaleMode(_scaleMode)
                .AttachTo(_parent)
                .AddClasses(_classes ?? Array.Empty<string>())
                .Build();
        }

        /// <summary>
        /// Resolves the <see cref="Texture2D"/> to use in the following order:
        /// 1. Explicitly set <see cref="_texture"/>
        /// 2. <see cref="_sprite"/> texture
        /// 3. Resource loaded from <see cref="_resourcePath"/>
        /// </summary>
        /// <returns>The resolved <see cref="Texture2D"/>, or null if none found.</returns>
        private Texture2D ResolveTexture() => _texture ?? _sprite?.texture ?? (!string.IsNullOrEmpty(_resourcePath) ? Resources.Load<Texture2D>(_resourcePath) : null);
    }
}