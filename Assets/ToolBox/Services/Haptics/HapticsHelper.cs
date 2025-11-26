using System;
using ToolBox.Messenger;

namespace ToolBox.Services.Haptics
{
    public static class HapticsHelper 
    {
        /// <summary>
        /// Request haptics .
        /// </summary>
        ///
        public static void RequestHaptics(HapticType type = HapticType.Low)
        {
            MessageBus.Instance.Broadcast(HapticsMessages.OnHapticsRequest, type);
        }
       
    }
}
