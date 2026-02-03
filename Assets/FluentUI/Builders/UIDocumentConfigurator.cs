using UnityEngine.UIElements;

namespace FluentUI.Builders
{
    public class UIDocumentConfigurator : FluentUI.Builders.BaseBuilder<VisualElement, UIDocumentConfigurator>
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
    }
}

