using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Pages
{
    public class TestPage : IPage
    {
        private VisualElement _root;
        
        public void Open(VisualElement root)
        {
            _root = root ?? throw new System.ArgumentNullException(nameof(root));
            
            
        }

        public void Close()
        {
            Logger.Log( $"Implement cleanup and orphan self" );
        }
    }
    
}
