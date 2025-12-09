using System.Collections;
using ToolBox.Messenger;
using ToolBox.Services;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Performance.Fps
{
    /// <summary>
    /// Controls the application's target frame rate via Application.targetFrameRate.
    /// Supports boosting, reducing, and resetting the frame rate in response to corresponding requests.
    /// </summary>
    
    public class FrameRateController : BaseService
    {
        [SerializeField] private int maxSupportedFrameRate = 60;
        [SerializeField] private int boostedFrameRate = 60;
        [SerializeField] private int defaultFrameRate = 30;
        [SerializeField] private int reducedFrameRate = 15;
        [SerializeField] private  float adjustmentDuration = 1f;
        
        private int _currentFrameRate;
        
        private Coroutine _adjustFrameRateCoroutine;
        
        private MessageBus _messageBus;
        
        private void Awake()
        {
            _messageBus = MessageBus.Instance;

            if (_messageBus == null)
            {
               
                Logger.Assert( false, "MessageBus null" );
            }
            
            SetFrameRate(defaultFrameRate);
        }
        
        private void BoostFrameRate() => StartFrameRateAdjustment(_currentFrameRate, Mathf.Min(boostedFrameRate, maxSupportedFrameRate));
        
        private void ReduceFrameRate() => StartFrameRateAdjustment(_currentFrameRate, reducedFrameRate);
        
        private void SetDefaultFrameRate() => SetFrameRate(defaultFrameRate);

        private void SetFrameRate(int frameRate) => Application.targetFrameRate = _currentFrameRate = frameRate;
        

        private void StartFrameRateAdjustment(int currentFrameRate, int targetFrameRate)
        {
            if (_adjustFrameRateCoroutine != null)
                StopCoroutine(_adjustFrameRateCoroutine);

            _adjustFrameRateCoroutine = StartCoroutine(AdjustFrameRateCoroutine(currentFrameRate, targetFrameRate, adjustmentDuration));
        }
        
        private IEnumerator AdjustFrameRateCoroutine(int start, int target, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                
                _currentFrameRate = Mathf.RoundToInt(Mathf.Lerp(start, target, elapsed / duration));
                Application.targetFrameRate = _currentFrameRate;
                yield return null;
            }

            Application.targetFrameRate = _currentFrameRate = target;
            _adjustFrameRateCoroutine = null;
        }
        
        protected override void SubscribeToService()
        {
            _messageBus.AddListener( FrameRateControllerMessages.BoostFrameRateMessage, BoostFrameRate );
            _messageBus.AddListener( FrameRateControllerMessages.ReduceFrameRateMessage, ReduceFrameRate );
            _messageBus.AddListener(FrameRateControllerMessages.SetDefaultFrameRateMessage, SetDefaultFrameRate );
        }

        protected override void UnsubscribeFromService()
        {
            _messageBus.RemoveListener( FrameRateControllerMessages.BoostFrameRateMessage, BoostFrameRate );
            _messageBus.RemoveListener( FrameRateControllerMessages.ReduceFrameRateMessage, ReduceFrameRate );
            _messageBus.RemoveListener( FrameRateControllerMessages.SetDefaultFrameRateMessage, SetDefaultFrameRate );
        }
    }

    public static class FrameRateControllerMessages
    {
        public const string BoostFrameRateMessage = "BoostFrameRate";
        public const string ReduceFrameRateMessage = "ReduceFrameRate";
        public const string SetDefaultFrameRateMessage = "SetDefaultFrameRate";
    }
}
