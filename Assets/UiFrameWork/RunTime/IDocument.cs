using CodeBase.Documents;
using UnityEngine.UIElements;

namespace UiFrameWork.RunTime
{
    public interface IDocument
    {
        void Build(VisualElement root);
        
        void Open(VisualElement root);

        void Close();

        public void OpenPage(PageID id);
        
        public void ClosePage(PageID id, VisualElement page);
        
        /// <summary>
        /// Indicates whether the document should be cached by the DocumentService.
        /// </summary>
        bool ShouldCache { get; }
        
    }
}
