using CodeBase.UiComponents.Factories;
using CodeBase.UiComponents.Footers;
using CodeBase.UiComponents.Headers;
using CodeBase.UiComponents.Styles;
using ToolBox.Messenger;
using UiFrameWork.Builders;
using UiFrameWork.RunTime;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents
{
    public class HubDocument : BaseDocument
    {
        protected override void Build()
        {
            DocumentRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.PageRoot).AttachTo(Root).Build();
            
            new DefaultHeader("Main Hub", DocumentRoot, () => { Logger.Log($"OnBack Selected"); }, Close);
            
            var container  = new ContainerBuilder().AddClass(UiStyleClassDefinitions.Container).AddClass(UiStyleClassDefinitions.ContainerRow).AttachTo(DocumentRoot).Build();
            
           ButtonFactory.CreateButton(ButtonType.Confirm, "DEMO-A",() => { MessageBus.Instance.Broadcast( nameof(DocumentFactoryMessages.OnRequestOpenDocument), DocumentID.DemoA); }, container).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           ButtonFactory.CreateButton(ButtonType.Confirm, "TEST PAGE",() => { MessageBus.Instance.Broadcast( nameof(DocumentFactoryMessages.OnRequestOpenDocument), DocumentID.TestDocument); }, container).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           ButtonFactory.CreateButton(ButtonType.Confirm, "2",() => { Logger.Log("2"); }, container).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           ButtonFactory.CreateButton(ButtonType.Confirm, "3",() => { Logger.Log("3"); }, container).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           ButtonFactory.CreateButton(ButtonType.Confirm, "4",() => { Logger.Log("4"); }, container).AddToClassList( UiStyleClassDefinitions.SpacedChild );
           
           new SingleButtonFooter(()=> { Logger.Log("Close"); }, "Close", DocumentRoot);
        }
    }
}
