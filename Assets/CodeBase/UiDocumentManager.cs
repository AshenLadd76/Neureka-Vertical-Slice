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

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            
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
    }
}
