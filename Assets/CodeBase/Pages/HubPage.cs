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

        private VisualElement _pageContainer;
        
        public void Open(VisualElement root)
        {
            _root = root;
            
            BuildHubPage();
        }

        public void Close()
        {
            if (_root == null || _pageContainer == null) return;
            
            MessageBus.Instance.Broadcast(nameof(PageFactoryMessages.OnRequestClosePage), PageID.HubPage);
            
            _root?.Remove( _pageContainer );
            
            _pageContainer = null;
        }

        private void BuildHubPage()
        {
            _pageContainer  = new ContainerBuilder()
                .AddClass(UiStyleClassDefinitions.Container)
                .AddClass(UiStyleClassDefinitions.ContainerRow)
                .AttachTo(_root)
                .Build();
            
            
           new DefaultHeader("Main Hub", _pageContainer, 
               () => { Logger.Log($"OnBack Selected"); },
               Close);

           //var container = new DefaultContainer(_root);
           
           
           ButtonFactory.CreateButton(ButtonType.Confirm, "1",() => { Logger.Log("1"); }, _pageContainer).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           ButtonFactory.CreateButton(ButtonType.Confirm, "2",() => { Logger.Log("2"); }, _pageContainer).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           ButtonFactory.CreateButton(ButtonType.Confirm, "3",() => { Logger.Log("3"); }, _pageContainer).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           ButtonFactory.CreateButton(ButtonType.Confirm, "4",() => { Logger.Log("4"); }, _pageContainer).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           
           new SingleButtonFooter(()=> { Logger.Log("Close"); }, "Close", _pageContainer);
           
        }
    }
}
