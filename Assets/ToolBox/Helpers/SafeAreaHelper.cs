using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Helpers
{
    [RequireComponent(typeof(UIDocument))]
    public class SafeAreaHelper : MonoBehaviour
    {
        
        private UIDocument _uiDocument;
        
         private VisualElement _rootVisualElement; // Assign your root VisualElement

        private Rect _lastSafeArea = Rect.zero;

        private void Awake()
        {
            _uiDocument  = GetComponent<UIDocument>();

            if (_uiDocument == null)
            {
                Logger.LogError("No UIDocument found!");
            }
        }


        private void Start()
        {
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            ApplySafeArea();
        }

        private void Update()
        {
            // Only update if safe area changed (orientation/device rotation)
            if (Screen.safeArea != _lastSafeArea)
            {
                ApplySafeArea();
            }
        }

        private void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;
            _lastSafeArea = safeArea;

            // Normalized offsets
            float top = 1f - safeArea.yMax / Screen.height;
            float bottom = safeArea.yMin / Screen.height;
            float left = safeArea.xMin / Screen.width;
            float right = 1f - safeArea.xMax / Screen.width;

            // Convert to pixels
            _rootVisualElement.style.paddingTop = safeArea.height * top + safeArea.yMin;
            _rootVisualElement.style.paddingBottom = safeArea.height * bottom;
            _rootVisualElement.style.paddingLeft = safeArea.width * left + safeArea.xMin;
            _rootVisualElement.style.paddingRight = safeArea.width * right;

            // Optional: Debug
            Logger.Log($"SafeArea applied: {safeArea}");
        }
    }
}