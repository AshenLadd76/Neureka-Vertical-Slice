using System;
using System.Collections.Generic;
using FluentUI.Components;
using FluentUI.Helpers.Editor;
using ToolBox.Editor;
using ToolBox.TileManagement.Editor.Styling;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UiFrameWork.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;
using CheckBoxBuilder = ToolBox.Editor.CheckBoxBuilder;

namespace ToolBox.TileManagement.Editor
{
    public class TileSetExtractor : EditorWindow
    {
        private const int MinTileSize = 8;
        private const int MaxTileSize = 1024;

        private int _tileMapChunkSizeX = 128;
        private int _tileMapChunkSizeY = 128;
        
        private int _tileWidth = 16;
        private int _tileHeight = 16;
        private int _tolerance;
        
        private Texture2D _loadedTexture;
        private Image _texturePreview;
        private Image _tileSetPreview;
        private Image _tileMapPreview;
        
        private string _currentTileMapName;
        
        private const string DefaultSavePath = "Assets/Sprites/TileMaps";
        
        private TextField _savePathTextField;
        
        private bool _addCollidersToTileMap = false;

        [MenuItem("Tools/Tile Set Extractor")]
        public static void ShowWindow()
        {
            GetWindow<TileSetExtractor>("TileSet Extractor");
        }
        
        
        public void CreateGUI()
        {
            UssStyleSheetLoader.Load(TileExtractorUss.UssFilePath, rootVisualElement);
            
            var root = rootVisualElement;
            
            root.AddToClassList( TileExtractorUss.Root );
            
            var mainPageContainer  = CreatePageLayout(root);
            
            //  AddChunkDropdown(root, val => { _tileMapChunkSizeX = val;  });
            //  AddChunkDropdown( root, val => { _tileMapChunkSizeY = val;  } );
        }

        private VisualElement CreatePageLayout(VisualElement root)
        {
            var mainScrollView = new ScrollViewBuilder().AddClass(TileExtractorUss.ScrollView).AttachTo(root).Build();
            
            //Main container
            var mainContainer = new ContainerBuilder().AddClass(TileExtractorUss.Main).OnGeometryChanged(
                evt =>
                {
                    
                    if (evt.target is not VisualElement container) return;
                    
                    if (Math.Abs(evt.newRect.width - evt.oldRect.width) < 0.1f) return;
                    
                    container.style.flexDirection = 
                        container.resolvedStyle.width < container.resolvedStyle.height
                            ? FlexDirection.Column
                            : FlexDirection.Row;
                }).Build();
            
            //Config
             var configContainer  = new ContainerBuilder().AddClass(TileExtractorUss.Column).AttachTo(mainContainer).Build();
             
             var config = new ContainerBuilder().AddClass(TileExtractorUss.Configuration).AttachTo(configContainer).Build();
             
             AddEditorFields(config);
             
             CreateAddCollidersCheckbox(config);
             
             CreateOutPutTextField(config);
             //Config ends
             
            var sourceImageContainer = new ContainerBuilder().AddClass(TileExtractorUss.Column).AttachTo(mainContainer).Build();
            CreatePreviewTexture( sourceImageContainer );
            
            var slicedImageContainer = new ContainerBuilder().AddClass(TileExtractorUss.Column).AttachTo(mainContainer).Build();
            _tileSetPreview = CreateImage(256, 256, _tileWidth, _tileHeight, TileExtractorUss.TexturePreview);
            slicedImageContainer.Add(_tileSetPreview);
            
            var tileMapImageContainer = new ContainerBuilder().AddClass(TileExtractorUss.Column).AttachTo(mainContainer).Build();
            _tileMapPreview = CreateImage(256, 256, _tileWidth, _tileHeight, TileExtractorUss.TexturePreview);
             tileMapImageContainer.Add(_tileMapPreview);
            
            
            mainScrollView.contentContainer.Add(mainContainer);
            
            return mainContainer;
        }
        
        private void CreateOutPutTextField(VisualElement root)
        {
            _savePathTextField = new TextField("Save Path")
            {
                value = DefaultSavePath,
                isReadOnly = false, // prevents editing, still selectable
            };

            _savePathTextField.AddToClassList( TileExtractorUss.TileExtractorSavePath );
            
            root.Add(_savePathTextField);
        }
        
        private void CreatePreviewTexture(VisualElement root)
        {
            _texturePreview = CreateImage( 256, 256, _tileWidth, _tileHeight, TileExtractorUss.TexturePreview );
            
            var overlay = DragDropOverlayFactory.CreateDragDropOverlay(TileExtractorUss.DragDropOverlay, RunTileExtractionPipeline  );
            
            _texturePreview.Add(overlay);
            
            root.Add(_texturePreview);
        }

        private Image CreateImage( int width, int height, int tileWidth, int tileHeight, string ussClass, ScaleMode scaleMode = ScaleMode.ScaleToFit )
        {
            var image = new Image
            {
                image = TextureLoader.CreateTexture(width, height, tileWidth, tileHeight),
                scaleMode = scaleMode
            };
            
            image.AddToClassList( ussClass );
            
            return image;
        }
        
        
        //Refactor
        private void AddEditorFields(VisualElement root)
        {
            root.Add( CreateIntegerField( "Tile Width" , _tileWidth, MinTileSize, MaxTileSize,  value => { _tileWidth = value; }));
            root.Add( CreateIntegerField( "Tile Height" , _tileHeight, MinTileSize, MaxTileSize, value => { _tileHeight = value; }));
            root.Add( CreateIntegerField( "Tolerance" , _tolerance, MinTileSize, MaxTileSize, value => { _tolerance = value; }));
        }

        private void AddChunkDropdown(VisualElement root, Action<int> action)
        {
            var options = new List<int> { 128, 256, 512, 1024 };
            int defaultOption = 128;

            var popup = new PopUpBuilder<int>()
                .Label("Select Tile Map Chunk Size")
                .Choices(options)
                .DefaultValue(defaultOption)
                .Size(400,20)
                .OnValueChanged( action )
                .Build();
            
                root.Add(popup);
        }

        
        private void CreateAddCollidersCheckbox(VisualElement root)
        {
            var collisionToggle = new CheckBoxBuilder( "Add Colliders", false )
                .OnValueChanged( SetTileMapCollisions )
                .Build();
            
            root.Add( collisionToggle );
        }

        private void SetTileMapCollisions(bool b)
        {
            Logger.Log( $"SetTileMapCollisions {b}" );
            _addCollidersToTileMap = b;
        }

        
        private IntegerField CreateIntegerField(string label, int initialValue, int min, int max, Action<int> onChanged)
        {
            if (string.IsNullOrEmpty(label)) return null;
            
            var integerField = new IntegerField(label) { value = initialValue };
            
            integerField.RegisterValueChangedCallback(evt => { onChanged(Mathf.Clamp(evt.newValue, min, max)); });
            
            return integerField;
        }
        
        
        private void RunTileExtractionPipeline(string path)
        {

            var tileMapPipeLineService = new TileMapPipelineService(
                path, _tileWidth, _tileHeight, _tileMapChunkSizeX, _tileMapChunkSizeY, _addCollidersToTileMap, _tileMapPreview );

            var  createdTextures=  tileMapPipeLineService.RunPipeLine();
            
             _texturePreview.image = createdTextures.loadedTexture;
            _tileSetPreview.image = createdTextures.spriteAtlasTexture;
            _tileMapPreview.image = createdTextures.loadedTexture;
            
            //ActivateTileMapGridOverlay();
        }

        // private void ActivateTileMapGridOverlay(int originalWidth = 4096, int originalHeight = 4096)
        // {
        //       new GridOverlayBuilder()
        //           .SetOriginalImageSize(originalWidth, originalWidth)  
        //         .SetWidth(512)
        //         .SetHeight(512)
        //         .CellSize(1024,1024) //Chunk size set by user....
        //         .SetLineColor(Color.red)
        //         .SetLineThickness(2f)
        //         .AttachTo(_tileMapPreview)
        //         .Build();
        // }
    }
    
}

