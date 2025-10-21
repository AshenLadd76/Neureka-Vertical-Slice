using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class UIDocumentConfigurator : BaseBuilder<VisualElement, UIDocumentConfigurator>
    {
        private UIDocument _uiDocument;

        public UIDocumentConfigurator(UIDocument uiDocument)
        {
            _uiDocument = uiDocument;
            VisualElement = _uiDocument.rootVisualElement; // use the existing root
        }

        public UIDocumentConfigurator SetUIDocument(UIDocument doc)
        {
            _uiDocument = doc;
            VisualElement = _uiDocument.rootVisualElement;
            return this;
        }

        // Example additional convenience
        // public UIDocumentBuilder ApplySafeArea()
        // {
        //     var safe = Screen.safeArea;
        //     _visualElement.style.paddingTop = safe.yMin;
        //     _visualElement.style.paddingBottom = Screen.height - safe.yMax;
        //     _visualElement.style.paddingLeft = safe.xMin;
        //     _visualElement.style.paddingRight = Screen.width - safe.xMax;
        //     return this;
        // }
    }
}

