using ToolBox.Messenger;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Services.Haptics
{
    public class HapticsService : MonoBehaviour
    {
        private const string HapticsKey = "Haptics";
        
        private bool _isSubscribed = false;
        
        private void OnEnable() => Subscribe();
        
        private void OnDisable() => UnSubscribe();
        


        private void HandleHaptics(HapticType hapticType)
        {
            Logger.Log($"Haptics: {hapticType}");
        }
        

        private void Subscribe()
        {
            if( _isSubscribed ) return;
            
            MessageBus.Instance.AddListener<HapticType>(HapticsKey, HandleHaptics );
            
            _isSubscribed = true;
        }

        private void UnSubscribe()
        {
            if(!_isSubscribed) return;
            
            MessageBus.Instance.RemoveListener<HapticType>(HapticsKey, HandleHaptics );
            
            _isSubscribed = false;
        }
            
    }

    public enum HapticType
    {
        Low,
        Medium,
        High
    }
}
