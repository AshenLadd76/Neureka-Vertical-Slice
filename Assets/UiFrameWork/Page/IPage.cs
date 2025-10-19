using CodeBase.Documents;
using UiFrameWork.RunTime;
using UnityEngine.UIElements;

namespace UiFrameWork.Page
{
    public interface IPage
    {
        public PageID PageIdentifier { get; set; }
        
        public void Open(VisualElement root, IDocument document);

        public void Close();
    }
}
