using System.Collections.Generic;
using ToolBox.Messenger;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Services.Haptics
{
    public class HapticsService : MonoBehaviour
    {
        private bool _isSubscribed = false;
        
        private readonly Dictionary<HapticType, HapticConfig> _hapticConfigs = new();
        
        private void OnEnable() => Subscribe();
        
        private void OnDisable() => UnSubscribe();

        private void Awake()
        {
            _hapticConfigs.Add( HapticType.Low, new HapticConfig( 50, 75 ) );
            _hapticConfigs.Add( HapticType.Medium, new HapticConfig( 200, 150 ) );
            _hapticConfigs.Add( HapticType.High, new HapticConfig( 250, 225 ) );
        }
        


        private void HandleHaptics(HapticType hapticType)
        {
            if (!AndroidHapticsWrapper.HasVibrator())
            {
                Logger.Log("This device does not have a vibration feature");
                return;
            }
            
            var config = _hapticConfigs[hapticType];
            
            AndroidHapticsWrapper.Vibrate(config.DurationMilliSeconds, config.Amplitude);
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

    public class HapticConfig
    {
        public HapticConfig(int durationMilliSeconds, int amplitude)
        {
            DurationMilliSeconds = durationMilliSeconds;
            Amplitude = amplitude;
        }
        
        public int DurationMilliSeconds { get; set; }
        public int Amplitude { get; set; }
        
    }
}
