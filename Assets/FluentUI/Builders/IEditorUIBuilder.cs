#if UNITY_EDITOR

using UnityEditor;

namespace FluentUI.Builders
{
    public interface IEditorUIBuilder 
    {
        IEditorUIBuilder SetEditorStyle(string styleName);
        void AttachToEditorWindow(EditorWindow window);
    }
}

#endif
