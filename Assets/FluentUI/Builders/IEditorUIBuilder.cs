#if UNITY_EDITOR

using UnityEditor;

namespace UiFrameWork.Builders
{
    public interface IEditorUIBuilder 
    {
        IEditorUIBuilder SetEditorStyle(string styleName);
        void AttachToEditorWindow(EditorWindow window);
    }
}

#endif
