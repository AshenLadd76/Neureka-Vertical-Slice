using System.Collections;
using ToolBox.Messenger;
using UnityEngine;


namespace ToolBox.Performance.Fps
{
    public class FrameRateController : MonoBehaviour
    {
        [SerializeField] private int maxSupportedFrameRate = 60;
        [SerializeField] private int boostedFrameRate = 60;
        [SerializeField] private int defaultFrameRate = 30;
        [SerializeField] private int reducedFrameRate = 15;
        
        private Coroutine _reductionCoroutine;
        
        private readonly float _reductionDuration = 1f;
        
        private bool _isSubscribed = false;
        
        private void OnEnable() => Subscribe();
        
        private void OnDisable() => UnSubscribe();
        
        private void Start() => SetDefaultFrameRate();

        private void BoostFrameRate()
        {
            if( _reductionCoroutine != null ) StopCoroutine( _reductionCoroutine );
            
            SetFrameRate(Mathf.Min(boostedFrameRate, maxSupportedFrameRate));
        }

        private void SetDefaultFrameRate() => SetFrameRate(defaultFrameRate);
        
        private void ReduceFrameRate() => StartFrameRateReduction(reducedFrameRate);
        
        private void SetFrameRate(int frameRate) => Application.targetFrameRate = frameRate;
        
        
        private void StartFrameRateReduction(int target)
        {
            if (_reductionCoroutine != null)
                StopCoroutine(_reductionCoroutine);

            _reductionCoroutine = StartCoroutine(ReduceFrameRateCoroutine(target, _reductionDuration));
        }


        private IEnumerator ReduceFrameRateCoroutine(int target, float duration)
        {
            int start = Application.targetFrameRate;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Application.targetFrameRate = Mathf.RoundToInt(Mathf.Lerp(start, target, elapsed / duration));
                yield return null;
            }

            Application.targetFrameRate = target;
            _reductionCoroutine = null;
        }
        

        private void Subscribe()
        {
            if (_isSubscribed) return;
            
            MessageBus.Instance.AddListener( FrameRateControllerMessages.BoostFrameRateMessage, BoostFrameRate );
            MessageBus.Instance.AddListener( FrameRateControllerMessages.ReduceFrameRateMessage, ReduceFrameRate );
            MessageBus.Instance.AddListener(FrameRateControllerMessages.SetDefaultFrameRateMessage, SetDefaultFrameRate );
            
            _isSubscribed = true;
        }

        private void UnSubscribe()
        {
            if (!_isSubscribed) return;
                
            MessageBus.Instance.RemoveListener( FrameRateControllerMessages.BoostFrameRateMessage, BoostFrameRate );
            MessageBus.Instance.RemoveListener( FrameRateControllerMessages.ReduceFrameRateMessage, ReduceFrameRate );
            MessageBus.Instance.RemoveListener( FrameRateControllerMessages.SetDefaultFrameRateMessage, SetDefaultFrameRate );
            
            _isSubscribed = false;
        }
    }

    public static class FrameRateControllerMessages
    {
        public const string BoostFrameRateMessage = "BoostFrameRate";
        public const string ReduceFrameRateMessage = "ReduceFrameRate";
        public const string SetDefaultFrameRateMessage = "SetDefaultFrameRate";
    }
}
