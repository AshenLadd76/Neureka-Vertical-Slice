using ToolBox.Messenger;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Services.Haptics
{
    public class HapticsService : MonoBehaviour
    {
 
        
        private bool _isSubscribed = false;
        
        private void OnEnable() => Subscribe();
        
        private void OnDisable() => UnSubscribe();
        


        private void HandleHaptics(HapticType hapticType)
        {
            Logger.Log("Handling Haptic Type: " + hapticType);


            if (!AndroidHapticsWrapper.HasVibrator())
            {
                Logger.Log("This device does not have a vibration feature");
                return;
            }
            else
            {
                Logger.Log("Good To go with the vibrations");
            }
            
            AndroidHapticsWrapper.Vibrate(200, 254);

        }
        

        private void Subscribe()
        {
            if( _isSubscribed ) return;
            
            MessageBus.Instance.AddListener<HapticType>(HapticsMessages.OnHapticsRequest, HandleHaptics );
            
            _isSubscribed = true;
        }

        private void UnSubscribe()
        {
            if(!_isSubscribed) return;
            
            MessageBus.Instance.RemoveListener<HapticType>(HapticsMessages.OnHapticsRequest, HandleHaptics );
            
            _isSubscribed = false;
        }
            
    }

    public enum HapticType
    {
        Low,
        Medium,
        High
    }

    public static class HapticsMessages
    {
        public const string OnHapticsRequest = "OnHapticsRequest";
    }
}
