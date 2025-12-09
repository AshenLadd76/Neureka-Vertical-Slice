using System.Collections.Generic;
using ToolBox.Messenger;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Services.Haptics
{
    public class HapticsService : BaseService
    {
        private readonly Dictionary<HapticType, HapticConfig> _hapticConfigs = new();
        
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
            
            if (!_hapticConfigs.TryGetValue(hapticType, out var config))
            {
                Logger.LogError($"No haptic config found for {hapticType}");
                return;
            }
            
            AndroidHapticsWrapper.Vibrate(config.DurationMilliSeconds, config.Amplitude);
        }
        
        protected override void SubscribeToService() => MessageBus.Instance.AddListener<HapticType>(HapticsMessages.OnHapticsRequest, HandleHaptics );
        
        protected override void UnsubscribeFromService() => MessageBus.Instance.RemoveListener<HapticType>(HapticsMessages.OnHapticsRequest, HandleHaptics );
    }
}
