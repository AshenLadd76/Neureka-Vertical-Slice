using ToolBox.Messaging;
using ToolBox.Utils.Validation;
using UiFrameWork.Components;
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
        public VisualElement SafeAreaContainer { get; private set; }

        private const string UssPath = "Uss/";

        [SerializeField] private DocumentID entryPointDocument;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();

            SafeAreaContainer = new ContainerBuilder().AddClass("safe-area-container")
                .AttachTo(_uiDocument.rootVisualElement).Build();

            //SafeAreaGeometryChanged();
            
            //listen for geometry change here.....

            ObjectValidator.Validate(_uiDocument);

            UssLoader.LoadAllUssFromFolder(_uiDocument.rootVisualElement, UssPath);
        }

        private void Start() => LoadEntryPointDocument();


        private void LoadEntryPointDocument()
        {
            //Call the default document
            MessageBus.Broadcast(nameof(DocumentServiceMessages.OnRequestOpenDocument), entryPointDocument);
        }

        private void SafeAreaGeometryChanged()
        {

            EventCallback<GeometryChangedEvent> onGeometryChanged = null;

            onGeometryChanged = evt =>
            {
                SafeAreaContainer.UnregisterCallback(onGeometryChanged);

                if (evt.newRect.width == 0 || evt.newRect.height == 0) return;


                var safeAreaRect = Screen.safeArea;
                // proceed with safe area logic

                var safeAreaCalculator = new SafeAreaCalculator();

                safeAreaCalculator.CalculateSafeArea(SafeAreaContainer, safeAreaRect);

            };
            
            // REGISTER the callback!
            SafeAreaContainer.RegisterCallback(onGeometryChanged);
        }
    }

    public class SafeAreaCalculator
    {
        public void CalculateSafeArea(VisualElement safeAreaContainer, Rect safeAreaRect)
        {
            if (safeAreaContainer == null)
            {
                Logger.LogError("SafeAreaContainer is null");
                return;
            }
            
            Logger.Log( $"SafeAreaCalculator.CalculateSafeArea({safeAreaRect})" );

            var leftOffset = safeAreaRect.x;
            var bottomOffset = safeAreaRect.y;
            var rightOffset = Screen.width - (safeAreaRect.x + safeAreaRect.width);
            var topOffset = Screen.height - (safeAreaRect.y + safeAreaRect.height);
            
            Logger.Log( $"L: { leftOffset } B: { bottomOffset } R: { rightOffset } T: { topOffset }" );
            
            safeAreaContainer.style.left = leftOffset;
            safeAreaContainer.style.right = rightOffset;
            safeAreaContainer.style.top = topOffset;
            safeAreaContainer.style.bottom = bottomOffset;
        }
    }
}
