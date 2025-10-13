using System.ComponentModel;
using CodeBase.UiComponents.Footers;
using ToolBox.Messenger;
using ToolBox.Utils.Validation;
using UiFrameWork.Builders;
using UiFrameWork.RunTime;
using UiFrameWork.Tools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase
{
    public class UiDocumentManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
       

        private bool _isSubscribed;
        
        private void OnEnable()
        {
            if (_isSubscribed) return;
            
            MessageBus.Instance.AddListener<VisualElement>(nameof(DocumentManagerMessages.OnAddDocument), OnAddDocument);
            MessageBus.Instance.AddListener<VisualElement>(nameof(DocumentManagerMessages.OnRemoveDocument), OnRemoveDocument);
            
            _isSubscribed = true;
        }

        private void OnDisable()
        {
            if (!_isSubscribed) return;
            
            MessageBus.Instance.RemoveListener<VisualElement>(nameof(DocumentManagerMessages.OnAddDocument), OnAddDocument);
            MessageBus.Instance.RemoveListener<VisualElement>(nameof(DocumentManagerMessages.OnRemoveDocument), OnRemoveDocument);
            
            _isSubscribed = false;
        }

        private void Awake()
        {
            
            
            _uiDocument = GetComponent<UIDocument>();
            
            ObjectValidator.Validate(_uiDocument);
            
            // Initialize the builder for the UIDocument root
            var rootBuilder = new UIDocumentConfigurator(_uiDocument)
                .SetWidthPercent(Length.Percent(100))
                .SetHeightPercent(Length.Percent(100))
                .SetFlexDirection(FlexDirection.Column)
                .SetAlignItems(Align.Stretch)
                .SetJustifyContent(Justify.FlexStart);
                
                //.ApplySafeArea();



            rootBuilder.AddChild(BuildHeader(100,10, Color.gray, 0f));
            rootBuilder.AddChild(BuildBody());
            rootBuilder.AddChild(new SingleButtonFooter(() => { Logger.Log($"I was clicked on, so im working ok"); }, $"Hit me!", "footer"));
            
            
            UssLoader.LoadAllUssFromFolder( _uiDocument.rootVisualElement, $"Uss/" );
        }



        private VisualElement BuildHeader(float width, float height, Color color, float padding)
        {
            var header =  new ContainerBuilder()
                .SetWidthPercent(Length.Percent(width))
                .SetHeightPercent(Length.Percent(height))
                .SetFlexDirection(FlexDirection.Row) // horizontal layout for nav items
                .SetAlignItems(Align.Center)  
                .SetBackgroundColor(color)
                .SetBorder(1)
                .SetBorderColor(Color.white)
                .SetBorderRadius(12)
                .SetPadding(padding, padding, padding, padding)
                .AddChild( HeaderButtonContainer() )
                .AddChild(HorizontalSpacer())
                .AddChild(HeaderButtonContainer())
                
                
                .Build();
            
            
            return header;
        }

        private VisualElement BuildBody()
        {
            return BuildPageComponent(100, 90, Color.gray, 4f);
        }

        private VisualElement BuildFooter(float width, float height, Color color, float padding)
        {
            var footer =  new ContainerBuilder()
                .SetWidthPercent(Length.Percent(width))
                .SetHeightPercent(Length.Percent(height))
                .SetFlexDirection(FlexDirection.Row) // horizontal layout for nav items
                .SetJustifyContent(Justify.Center)
                .SetAlignItems(Align.Center)  
                .SetBackgroundColor(color)
                .SetBorder(1)
                .SetBorderColor(Color.white)
                .SetBorderRadius(12)
                .SetPadding(padding, padding, padding, padding)
              //  .AddChild(HorizontalSpacer())
                .AddChild(BuildButton())
                .Build();
            
            return footer;
        }

       
        private VisualElement BuildButton(VisualElement parent = null)
        {
            float dpiScale = Screen.dpi / 100f;
            
            return new ButtonBuilder()
                .SetText("Test Button Builder")
                .SetWidth(500 * dpiScale)
                .SetHeight(100  * dpiScale)
                .OnClick(() => { MessageBus.Instance.Broadcast( nameof(PageFactoryMessages.OnRequestPage), PageID.TestPage ); })
                .SetFlexShrink(0)
                .AddClass( "btn-base" )
                //.AddClass("btn-large")
                //.AttachTo( parent )
                .Build();
        }

        private VisualElement HorizontalSpacer()
        {
            return new ContainerBuilder().SetFlexDirection(FlexDirection.Row).SetFlexGrow(0)
                .SetWidthPercent(Length.Percent(80))
                .SetHeightPercent(Length.Auto())
                .SetPadding(0,0,0,0)
                .Build();
        }

        private VisualElement HeaderButtonContainer()
        {
            return  new ContainerBuilder()
                .SetFlexDirection(FlexDirection.Column)
                .SetFlexGrow(0)
                .SetWidthPercent(Length.Percent(15))
                .SetMinWidthPercent(Length.Percent(5))
                .SetHeightPercent(Length.Percent(100))
                .SetMinHeightPercent(Length.Percent(100))
                .SetBorderColor(Color.white)
                .SetBackgroundColor(Color.black)
                .SetMargin(2,8,2,2)
                .SetPadding(0f,0f,0f,0f)
                .Build();
        }
        
        

        private VisualElement BuildPageComponent(float width, float height, Color color, float padding)
        {
            return new ContainerBuilder()
                .SetWidthPercent(Length.Percent(width))
                .SetHeightPercent(Length.Percent(height))
                .SetFlexDirection(FlexDirection.Row) // horizontal layout for nav items
                .SetAlignItems(Align.Center)  
                .SetBackgroundColor(color)
                .SetBorder(2)
                .SetBorderColor(Color.white)
                .SetBorderRadius(5)
                .SetPadding(padding, padding, padding, padding)
                .Build();
        }
        
            

        private void OnAddDocument(VisualElement visualElement)
        {
            Logger.Log("OnAddDocument");
        }

        private void OnRemoveDocument(VisualElement visualElement)
        {
            Logger.Log("OnRemoveDocument");
        }
        
    }

    public enum DocumentManagerMessages
    {
        OnAddDocument,
        OnRemoveDocument,
    }
}
