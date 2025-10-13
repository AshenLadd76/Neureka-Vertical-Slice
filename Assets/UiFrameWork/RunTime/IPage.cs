using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.RunTime
{
    public interface IPage
    {
        void Open(VisualElement root);

        void Close();


    }
}
