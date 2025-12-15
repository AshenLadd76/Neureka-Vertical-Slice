using CodeBase.Services;
using CodeBase.UiComponents.Factories;
using CodeBase.UiComponents.Footers;
using CodeBase.UiComponents.Headers;
using CodeBase.UiComponents.Styles;
using ToolBox.Messaging;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents
{
    public class HubDocument : BaseDocument
    {
        protected override void Build()
        {
            DocumentRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.PageRoot).AttachTo(Root).Build();
            
            new DefaultHeader("Demo Hub", DocumentRoot, () => { Logger.Log($"OnBack Selected"); }, Close, DemoHubUssDefinitions.Header, DemoHubUssDefinitions.HeaderButton, DemoHubUssDefinitions.HeaderLabel);
            
            var container  = new ContainerBuilder().AddClass(DemoHubUssDefinitions.Container).AddClass(DemoHubUssDefinitions.ContainerRow).AttachTo(DocumentRoot).Build();
            
           ButtonFactory.CreateButton(ButtonType.Confirm, "Risk Factors",() => { MessageBus.Broadcast( nameof(DocumentServiceMessages.OnRequestOpenDocument), DocumentID.Neureka); }, container).AddToClassList( DemoHubUssDefinitions.MenuButton );
           ButtonFactory.CreateButton(ButtonType.Confirm, "Main Hub",() => { MessageBus.Broadcast( nameof(DocumentServiceMessages.OnRequestOpenDocument), DocumentID.Hub); }, container).AddToClassList( DemoHubUssDefinitions.MenuButton );
           ButtonFactory.CreateButton(ButtonType.Confirm, "Depression Questionnaire",() => {  MessageBus.Broadcast(QuestionnaireService.OnRequestQuestionnaireMessage, "CESD-20"); }, container).AddToClassList( DemoHubUssDefinitions.MenuButton );
           ButtonFactory.CreateButton(ButtonType.Confirm, "AQ",() => { MessageBus.Broadcast( QuestionnaireService.OnRequestQuestionnaireMessage, "AQ" ); }, container).AddToClassList( DemoHubUssDefinitions.MenuButton );
           ButtonFactory.CreateButton(ButtonType.Confirm, "4",() => { Logger.Log("4"); }, container).AddToClassList( DemoHubUssDefinitions.MenuButton );
           
           new SingleButtonFooter(()=> { Logger.Log("Close"); }, "Close", DocumentRoot);
        }
    }

    public static class DemoHubUssDefinitions
    {
        public const string Container = "demo-container";
        public const string ContainerRow = "demo-container-row";
        public const string Header = "demo-header";
        public const string HeaderLabel = "demo-header-label";
        public const string HeaderButton = "demo-header-button";
        public const string MenuButton = "demo-menu-button";
    }
}
