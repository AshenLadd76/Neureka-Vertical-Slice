using CodeBase.UiComponents.Headers;
using CodeBase.UiComponents.Page;
using CodeBase.UiComponents.Styles;
using FluentUI.Components;
using ToolBox.Messaging;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents
{
    public class TestDocument : BaseDocument
    {
        private VisualElement _root;
        
        private VisualElement _documentRoot;
        
        public override void Close()
        {
            if (_documentRoot == null) return;
            
            MessageBus.Broadcast(nameof(DocumentServiceMessages.OnRequestCloseDocument), DocumentID.Neureka);
            
            _root?.Remove( _documentRoot );
            
            _documentRoot = null;
        }


        public override void Build(VisualElement root)
        {
            var sprite = Resources.Load<Sprite>($"Sprites/MotherShip");
            
            if( sprite == null ) Logger.Log("Sprite not found");
            
            
            _documentRoot = new ContainerBuilder()
                .AddClass( UiStyleClassDefinitions.PageRoot )
                .AttachTo(_root)
                .Build();
            
            new DefaultHeader("Test Document", _documentRoot, 
                Close,
                Close);
            
            var container  = new ContainerBuilder()
                .AddClass(UiStyleClassDefinitions.Container)
                .AddClass(UiStyleClassDefinitions.ContainerRow)
                .AttachTo(_documentRoot)
                .Build();
            
            
            new ImageBuilder()
                .SetSprite(sprite)
                .SetScaleMode(ScaleMode.ScaleToFit)
             
                .AttachTo(container)
                .Build();
            
            new SingleButtonFooter(()=> { Logger.Log("Close"); }, "Close", _documentRoot);
        }
        
    }
}
