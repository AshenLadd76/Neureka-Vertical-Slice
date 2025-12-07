using UnityEngine;

namespace ToolBox.Services.Haptics
{
    public static class AndroidHapticsWrapper
    {
        private static AndroidJavaObject _plugin;
        
        private static bool? _hasVibratorCache;

        static AndroidHapticsWrapper()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    _plugin = new AndroidJavaObject("com.pushtest.unityhapticplugin.HapticsPlugin", activity);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to initialize HapticsPlugin: {e}");
            }
#endif
        }

        public static bool HasVibrator()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (_plugin == null) return false;

             if (_hasVibratorCache.HasValue) return _hasVibratorCache.Value;

             _hasVibratorCache = _plugin.Call<bool>("HasVibrator");

            return _hasVibratorCache.Value;
#else
            return false;
#endif
        }

        public static void Vibrate(int durationMs, int amplitude)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (_plugin == null)
            {
                Debug.LogWarning("HapticsPlugin not initialized.");
                return;
            }

            if (!HasVibrator())
            {
                Debug.Log("Device has no vibrator.");
                return;
            }

            _plugin.Call("Vibrate", durationMs, amplitude);
#else
            Debug.Log($"[Editor] Vibrate({durationMs}ms)");
#endif
        }
    }
}

