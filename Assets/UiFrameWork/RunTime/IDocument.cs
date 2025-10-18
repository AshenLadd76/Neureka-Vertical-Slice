using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.RunTime
{
    public interface IDocument
    {
        void Open(VisualElement root);

        void Close();
        
    }
}
