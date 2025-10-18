using UnityEngine.UIElements;

namespace UiFrameWork.Page
{
    public interface IPage
    {
        public void Open(VisualElement root);

        public void Close();
    }
}
