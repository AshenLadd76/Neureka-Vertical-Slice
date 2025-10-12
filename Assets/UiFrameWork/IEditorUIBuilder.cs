#if UNITY_EDITOR

using UnityEditor;

namespace UiFrameWork
{
    public interface IEditorUIBuilder 
    {
        IEditorUIBuilder SetEditorStyle(string styleName);
        void AttachToEditorWindow(EditorWindow window);
    }
}

#endif
