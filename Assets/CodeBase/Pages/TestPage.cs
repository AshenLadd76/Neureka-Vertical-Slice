using CodeBase.UiComponents.Footers;
using CodeBase.UiComponents.Headers;
using CodeBase.UiComponents.Styles;
using ToolBox.Messenger;
using UiFrameWork.Builders;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Pages
{
    public class TestPage : IPage
    {
        private VisualElement _root;
        
        private VisualElement _pageRoot;
        
        
        
        public void Open(VisualElement root)
        {
            _root = root ?? throw new System.ArgumentNullException(nameof(root));
            
            BuildTestPage();
            
            
        }

        public void Close()
        {
            if (_pageRoot == null) return;
            
            MessageBus.Instance.Broadcast(nameof(PageFactoryMessages.OnRequestClosePage), PageID.TestPage);
            
            _root?.Remove( _pageRoot );
            
            _pageRoot = null;
        }


        private void BuildTestPage()
        {
            var sprite = Resources.Load<Sprite>($"Sprites/MotherShip");
            
            if( sprite == null ) Logger.Log("Sprite not found");
            
            
            _pageRoot = new ContainerBuilder()
                .AddClass( UiStyleClassDefinitions.PageRoot )
                .AttachTo(_root)
                .Build();
            
            new DefaultHeader("Test Page", _pageRoot, 
                Close,
                Close);
            
            var container  = new ContainerBuilder()
                .AddClass(UiStyleClassDefinitions.Container)
                .AddClass(UiStyleClassDefinitions.ContainerRow)
                .AttachTo(_pageRoot)
                .Build();
            
            
            new ImageBuilder()
                .SetSprite(sprite)
                .SetScaleMode(ScaleMode.ScaleToFit)
             
                .AttachTo(container)
                .Build();
            
            new SingleButtonFooter(()=> { Logger.Log("Close"); }, "Close", _pageRoot);
        }
        
    }
    
}
