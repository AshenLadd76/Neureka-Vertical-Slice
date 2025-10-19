using CodeBase.Documents;
using UnityEngine.UIElements;

namespace UiFrameWork.RunTime
{
    public interface IDocument
    {
        void Open(VisualElement root);

        void Close();

        public void OpenPage(PageID id);
        
        public void ClosePage(PageID id, VisualElement page);

    }
}
