using System;
using System.Collections.Generic;
using System.IO;
using ToolBox.TileManagement.TileExtraction;
using UiFrameWork.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ToolBox.TileManagement.Editor
{
    public class TileMapPipelineService
    {
        private readonly string _path;
        private readonly int _tileWidth;
        private readonly int _tileHeight;
    
        private readonly int _chunkWidth;
        private readonly int _chunkHeight;
        private readonly bool _addCollidersToTileMap;
        
        private Texture2D _loadedTexture;
        private Texture2D _tileSet;
        
        private int _loadedTextureWidth = 512;
        private int _loadedTextureHeight = 512;
       
        
        private string _currentTileMapName;
        private string _spriteSheetName;

        private VisualElement _parent;
        
        
        private List<Color32[]> _uniqueSlicedTiles = new();


        private delegate void PipeLineStage();

        private void RunStages(params PipeLineStage[] stages)
        {
            foreach (var stage in stages)
                stage();
        }

       
        public TileMapPipelineService(
            string path,
            int tileWidth,
            int tileHeight,
            int chunkWidth,
            int chunkHeight,
            bool addCollidersToTileMap,
            VisualElement parent
            )
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            if (!File.Exists(path))
                throw new FileNotFoundException("File does not exist at the specified path.", path);

            _path = path;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _chunkWidth = chunkWidth;
            _chunkHeight = chunkHeight;
            _addCollidersToTileMap = addCollidersToTileMap;
            _parent = parent;
        }
       
        
        private void LoadTexture()
        {
            // Optional: check supported extensions
            string extension = Path.GetExtension(_path)?.ToLowerInvariant();
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
                throw new NotSupportedException($"Unsupported texture file type: {extension}. Supported: .png, .jpg, .jpeg");

            _loadedTexture = TextureLoader.LoadTextureFromFile(_path);
            
            
            if (_loadedTexture == null)
                throw new InvalidOperationException($"Failed to load texture from {_path}");
            
            
            _loadedTextureWidth = _loadedTexture.width;
            _loadedTextureHeight = _loadedTexture.height;
            
            _currentTileMapName = Path.GetFileNameWithoutExtension(_path);

            
        }
        
        private void ExtractUniqueTiles()
        {
            ITileExtractor tileExtractor = new TileExtractor(_loadedTexture, _tileWidth, _tileHeight, _currentTileMapName);
            _uniqueSlicedTiles = tileExtractor.ExtractTiles() ?? new List<Color32[]>();

            if (_uniqueSlicedTiles.Count == 0)
                Debug.LogError("No unique tiles found! Probably due to the image size.");
        }

     
        
        
        private void CreateSpriteSheet()
        {
            int columns = Mathf.CeilToInt(Mathf.Sqrt( _uniqueSlicedTiles.Count)); // square-ish
            int rows = Mathf.CeilToInt((float) _uniqueSlicedTiles.Count / columns);

            _tileSet = new TileSetBuilder(_uniqueSlicedTiles, _tileWidth, _tileHeight).SetColumnCount(columns).SetRowCount(rows).Build();
            
            SaveTileSet(_tileSet, _currentTileMapName );
            
        }
        
        private void SliceTileSet()
        {
            SpriteAssetSlicer.Slice( $"{EditorPrefs.GetString(TileExtractorKeys.SavePathKey)}/{_currentTileMapName}.png", _tileWidth,_tileHeight);
        }
        
        private void CreateTileMap()
        {
            var jsonPath = $"{EditorPrefs.GetString(TileExtractorKeys.SavePathKey)}/{_currentTileMapName}.json";
            var atlasPath = $"{EditorPrefs.GetString(TileExtractorKeys.SavePathKey)}/{_currentTileMapName}.png";
            
            Vector2Int chunkSize = new Vector2Int(_chunkWidth, _chunkHeight);

            ITileMapBuilder tileMapBuilder = new TileMapBuilder(jsonPath, atlasPath, _currentTileMapName, chunkSize, _addCollidersToTileMap, CompositeCollider2D.GeometryType.Outlines);
            
            tileMapBuilder.Build();
            
            ActivateTileMapGridOverlay( _loadedTextureWidth, _loadedTextureHeight );
            
        }


        private GridOverlayBuilder _gridOverlayBuilder;
        private void ActivateTileMapGridOverlay(int originalWidth = 4096, int originalHeight = 4096)
        {
            if (_gridOverlayBuilder != null)
            {
                _gridOverlayBuilder.Refresh(originalWidth, originalHeight, _tileWidth, _tileHeight);
                return;
            }

            _gridOverlayBuilder = new GridOverlayBuilder()
                .SetOriginalImageSize(originalWidth, originalHeight)  
                .SetWidth(512) //Default preview image width
                .SetHeight(512) //Default preview image height
                .CellSize(384,1180) //Chunk size set by user....defined by dropdown
                .SetLineColor(Color.red)
                .SetLineThickness(2f);
            
                _gridOverlayBuilder.AttachTo(_parent).Build();
        }

        
        private void SaveTileSet(Texture2D texture, string filename)
        {
            byte[] pngData = texture.EncodeToPNG();

            var path = $"{EditorPrefs.GetString(TileExtractorKeys.SavePathKey)}/";
            
            if( !Directory.Exists(path) )
                Directory.CreateDirectory(path);
            
            File.WriteAllBytes( $"{path}{filename}.png", pngData);
            
            AssetDatabase.Refresh();
        }

        public (Texture2D loadedTexture, Texture2D spriteAtlasTexture, Texture2D tileMapTexture ) RunPipeLine()
        {
            RunStages(LoadTexture, ExtractUniqueTiles, CreateSpriteSheet, SliceTileSet, CreateTileMap);
            
            return ( _loadedTexture, _tileSet, _tileSet );
        }

        public ( Texture2D spriteAtlasTexture, Texture2D tileMapTexture ) ReSliceTiles()
        {
            if (_loadedTexture == null) throw new InvalidOperationException("Texture must be loaded before slicing.");
            
            RunStages(ExtractUniqueTiles, CreateSpriteSheet, SliceTileSet, CreateTileMap);
            
            return (_tileSet, _tileSet );
        }
    }
}