using System;
using ToolBox.Editor;
using ToolBox.Extensions;
using UnityEditor;
using UnityEngine.UIElements;

namespace ToolBox.TileManagement.Editor
{
    public static class DragDropOverlayFactory
    {
        public static VisualElement CreateDragDropOverlay(string ussClassName, Action<string> onFileDropped)
        {
            var overlay = new VisualElement()
            {
                pickingMode = PickingMode.Position
            };
            
            overlay.AddToClassList(ussClassName);
            overlay.StretchToParentSize();
            
            DragDropHandler.RegisterDropArea(overlay, 
                obj =>
                {
                    if (obj.IsNullOrEmpty())
                        return;
                
                    var path = AssetDatabase.GetAssetPath(obj[0]);
                    onFileDropped?.Invoke(path);
                },
                onFileDropped);
            
            return overlay;
        }
    }
}