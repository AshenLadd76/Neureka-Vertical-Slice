using System.Collections;
using ToolBox.Messenger;
using ToolBox.Utils.Validation;
using UiFrameWork.Builders;
using UiFrameWork.RunTime;
using UiFrameWork.Tools;
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
            
            
            UssLoader.LoadAllUssFromFolder( _uiDocument.rootVisualElement, $"Uss/" );
            
            // Initialize the builder for the UIDocument root
            var rootBuilder = new UIDocumentConfigurator(_uiDocument)
                .SetWidthPercent(Length.Percent(100))
                .SetHeightPercent(Length.Percent(100))
                .SetFlexDirection(FlexDirection.Column)
                .SetAlignItems(Align.Stretch)
                .SetJustifyContent(Justify.FlexStart);
        }


        private void Start()
        {
            MessageBus.Instance.Broadcast( nameof(PageFactoryMessages.OnRequestOpenPage), PageID.HubPage );
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
