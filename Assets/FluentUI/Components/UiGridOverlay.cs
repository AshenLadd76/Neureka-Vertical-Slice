using UnityEngine;
using UnityEngine.UIElements;

namespace FluentUI.Components
{
    public class GridOverlayBuilder : FluentUI.Builders.BaseBuilder<VisualElement, GridOverlayBuilder>
    {

        private int _originalImageWidth;
        private int _originalImageHeight;
        
        private int _previewCanvasWidth = 512; 
        private int _previewCanvasHeight = 512;

        private float _uniformScale;

        private int _previewImageWidth;
        private int _previewImageHeight;

        private int _offsetX;
        private int _offsetY;
        
        private float _scaledCellWidth;
        private float _scaledCellHeight;
        
        private int _cellWidth = 32;
        private int _cellHeight = 32;
        private Color _lineColor = Color.green;
        private float _lineThickness = 1f;

        public GridOverlayBuilder SetOriginalImageSize(int width, int height)
        {
            _originalImageWidth = width;
            _originalImageHeight = height;
            return this;
        }
        
        public GridOverlayBuilder SetWidth(int width)
        {
            _previewCanvasWidth = width;
            return this;
        }

        public GridOverlayBuilder SetHeight(int height)
        {
            _previewCanvasHeight = height;
            return this;
        }

      
        public GridOverlayBuilder CellSize(int cellWidth, int cellHeight)
        {
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;

            return this;
        }
        
        public GridOverlayBuilder SetLineColor(Color color)
        {
            _lineColor = color;
            return this;
        }

        public GridOverlayBuilder SetLineThickness(float lineThickness)
        {
            _lineThickness = lineThickness;
            return this;
        }


        public new VisualElement Build()
        {
            CalculateScaleFactor();
     
            VisualElement.generateVisualContent -= Draw; // unsubscribe first
            VisualElement.generateVisualContent += Draw;
            
            return VisualElement;
        }
        
        public void Refresh(int newImageWidth, int newImageHeight, int newCellWidth, int newCellHeight)
        {
            // Update the canvas size
            _previewCanvasWidth = newImageWidth;
            _previewCanvasHeight = newImageHeight;
            
            // Update the tile/chunk size
            _cellWidth = newCellWidth;
            _cellHeight = newCellHeight;

            // Recalculate all scale factors, preview size, offsets, and scaled cell size
            CalculateScaleFactor();       
            
            // Trigger the Draw() method without rebuilding
            VisualElement.MarkDirtyRepaint();  // triggers Draw()
        }


        private void CalculateScaleFactor()
        {
            if (_originalImageWidth == 0 || _originalImageHeight == 0)
                throw new System.InvalidOperationException("Original image size must be set before building GridOverlay.");
            
            var scaledFactorWidth = (float)_previewCanvasWidth / _originalImageWidth; 
            var scaledFactorHeight = (float)_previewCanvasHeight / _originalImageHeight;
            
            _uniformScale = Mathf.Min(scaledFactorWidth, scaledFactorHeight);
            
            _previewImageWidth = Mathf.RoundToInt(_originalImageWidth * _uniformScale);
            _previewImageHeight = Mathf.RoundToInt(_originalImageHeight * _uniformScale);
            
            _offsetX = (_previewCanvasWidth - _previewImageWidth) / 2;
            _offsetY = (_previewCanvasHeight - _previewImageHeight) / 2;

            
            _scaledCellWidth = _cellWidth * _uniformScale;
            _scaledCellHeight = _cellHeight * _uniformScale;
        }

        private void Draw(MeshGenerationContext ctx)
        {
            var painter = ctx.painter2D;

            // Set drawing properties
            painter.lineWidth = _lineThickness;
            painter.strokeColor = _lineColor;
            
            var scaledCellWidth = Mathf.RoundToInt(_scaledCellWidth);
            var scaledCellHeight = Mathf.RoundToInt(_scaledCellHeight);

            // Draw vertical lines
            for (int x = 0; x <= _previewImageWidth; x += scaledCellWidth)
            {
                painter.BeginPath();
                painter.MoveTo(new Vector2(x + _offsetX, _offsetY));
                painter.LineTo(new Vector2(x + _offsetX, _previewImageHeight + _offsetY));
                painter.Stroke();
            }

            // Draw horizontal lines
            for (int y = 0; y <= _previewImageHeight; y += scaledCellHeight)
            {
                painter.BeginPath();
                painter.MoveTo(new Vector2(_offsetX, y + _offsetY));
                painter.LineTo(new Vector2(_previewImageWidth + _offsetX , y + _offsetY));
                painter.Stroke();
            }
        }
    }
}
