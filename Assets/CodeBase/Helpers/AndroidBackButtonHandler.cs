using UnityEngine;
using Logger = ToolBox.Utils.Logger;

// assuming you use your MessageBus for navigation

namespace CodeBase.Helpers
{
    public class AndroidBackButtonHandler : MonoBehaviour
    {
        [SerializeField] private UiDocumentManager uiDocumentManager;

        private void Update()
        {
#if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HandleBackButton();
            }
#endif
        }

        private void HandleBackButton()
        {
            if (!uiDocumentManager)
            {
                Logger.LogWarning("UiDocumentManager not assigned, quitting by default.");
                Application.Quit();
                return;
            }
        
            Application.Quit();
            Logger.Log("Back button pressed on root – quitting app.");
           
        }
    }
}

