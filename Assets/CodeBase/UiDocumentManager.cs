using CodeBase.Services;
using FluentUI.Components;
using FluentUI.Tools;
using ToolBox.Messaging;
using ToolBox.Utils.Validation;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase
{
    public class UiDocumentManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        public VisualElement SafeAreaContainer { get; private set; }

        private const string UssPath = "Uss/";
        private const string SafeAreaContainerName = "safe-area-container";

        [SerializeField] private DocumentID entryPointDocument;

        private VisualElement _root;
        
        public const string ToggleUiMessage = "ToggleUI";

        private void OnEnable() =>MessageBus.AddListener( ToggleUiMessage, ToggleUi );
        

        private void OnDisable() => MessageBus.RemoveListener( ToggleUiMessage, ToggleUi );
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            _root = _uiDocument.rootVisualElement;
            
            ObjectValidator.Validate(this, this, true);

            SafeAreaContainer = new ContainerBuilder().AddClass(SafeAreaContainerName)
                .AttachTo(_uiDocument.rootVisualElement).Build();
            
            UssLoader.LoadAllUssFromFolder(_uiDocument.rootVisualElement, UssPath);
        }

        private void Start() => LoadEntryPointDocument();


        private void LoadEntryPointDocument()
        {
            //Call the default document
            MessageBus.Broadcast(nameof(DocumentServiceMessages.OnRequestOpenDocument), entryPointDocument);
        }

        private bool _isUiActive = true;
        private void ToggleUi()
        {
            _isUiActive = !_isUiActive;
            _root.style.display = _isUiActive ? DisplayStyle.Flex : DisplayStyle.None;
        }

    }
}
