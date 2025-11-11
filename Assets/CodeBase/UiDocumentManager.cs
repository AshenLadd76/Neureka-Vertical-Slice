using ToolBox.Messenger;
using ToolBox.Utils.Validation;
using UiFrameWork.RunTime;
using UiFrameWork.Tools;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase
{
    public class UiDocumentManager : MonoBehaviour
    {
        private UIDocument _uiDocument;

        private const string UssPath = "Uss/";

        [SerializeField] private DocumentID entryPointDocument;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();

            ObjectValidator.Validate(_uiDocument);

            UssLoader.LoadAllUssFromFolder(_uiDocument.rootVisualElement, UssPath);
        }

        private void Start() => LoadEntryPointDocument();
        
        
        private void LoadEntryPointDocument()
        {
            //Load the default document, main hub
            MessageBus.Instance.Broadcast(nameof(DocumentServiceMessages.OnRequestOpenDocument), entryPointDocument);
        }
    }
}
