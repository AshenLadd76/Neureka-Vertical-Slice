using System;
using ToolBox.Messaging;

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
            MessageBus.Broadcast(HapticsMessages.OnHapticsRequest, type);
        }
       
    }
}
