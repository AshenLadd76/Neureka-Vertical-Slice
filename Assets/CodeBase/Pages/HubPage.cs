using CodeBase.UiComponents.Factories;
using CodeBase.UiComponents.Footers;
using CodeBase.UiComponents.Headers;
using CodeBase.UiComponents.Styles;
using ToolBox.Messenger;
using UiFrameWork.Builders;
using UiFrameWork.RunTime;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Pages
{
    public class HubPage : IPage
    {
        private VisualElement _root;

        private VisualElement _pageRoot;
        
        public void Open(VisualElement root)
        {
            _root = root;
            
            BuildHubPage();
        }

        public void Close()
        {
            if (_pageRoot == null) return;
            
            MessageBus.Instance.Broadcast(nameof(PageFactoryMessages.OnRequestClosePage), PageID.HubPage);
            
            _root?.Remove( _pageRoot );
            
            _pageRoot = null;
        }

        private void BuildHubPage()
        {
            _pageRoot = new ContainerBuilder()
                .AddClass(UiStyleClassDefinitions.PageRoot)
                .AttachTo(_root)
                .Build();
            
            new DefaultHeader("Main Hub", _pageRoot, 
                () => { Logger.Log($"OnBack Selected"); },
                Close);
            
            
            var container  = new ContainerBuilder()
                .AddClass(UiStyleClassDefinitions.Container)
                .AddClass(UiStyleClassDefinitions.ContainerRow)
                .AttachTo(_pageRoot)
                .Build();
            
           ButtonFactory.CreateButton(ButtonType.Confirm, "1",() => { MessageBus.Instance.Broadcast(nameof(PageFactoryMessages.OnRequestOpenPage), PageID.TestPage); }, container).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           ButtonFactory.CreateButton(ButtonType.Confirm, "2",() => { Logger.Log("2"); }, container).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           ButtonFactory.CreateButton(ButtonType.Confirm, "3",() => { Logger.Log("3"); }, container).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           ButtonFactory.CreateButton(ButtonType.Confirm, "4",() => { Logger.Log("4"); }, container).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           
           new SingleButtonFooter(()=> { Logger.Log("Close"); }, "Close", _pageRoot);
        }
    }
}
