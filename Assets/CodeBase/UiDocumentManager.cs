using ToolBox.Messaging;
using ToolBox.Utils.Validation;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UiFrameWork.Tools;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase
{
    public class UiDocumentManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        public VisualElement SafeAreaContainer { get; private set; }

        private const string UssPath = "Uss/";

        [SerializeField] private DocumentID entryPointDocument;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();

            SafeAreaContainer = new ContainerBuilder().AddClass("safe-area-container")
                .AttachTo(_uiDocument.rootVisualElement).Build();
            
            ObjectValidator.Validate(_uiDocument);

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
