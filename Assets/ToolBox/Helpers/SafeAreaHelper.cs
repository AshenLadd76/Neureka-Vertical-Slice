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
            if (Screen.safeArea == _lastSafeArea) return;
            
            ApplySafeArea();
            
        }

        private void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;
            _lastSafeArea = safeArea;

            // Set padding in pixels correctly
            _rootVisualElement.style.paddingTop = safeArea.yMin;
            _rootVisualElement.style.paddingBottom = Screen.height - safeArea.yMax;
            _rootVisualElement.style.paddingLeft = safeArea.xMin;
            _rootVisualElement.style.paddingRight = Screen.width - safeArea.xMax;

            Logger.Log($"SafeArea applied: {safeArea}");
        }
    }
}