using System.Collections;
using ToolBox.Messenger;
using UnityEngine;

namespace ToolBox.Performance.Fps
{
    public class FrameRateController : MonoBehaviour
    {
        [SerializeField] private int defaultFrameRate = 60;
        [SerializeField] private int boostedFrameRate = 120;
        [SerializeField] private int reducedFrameRate = 30;
        
        private Coroutine _reductionCoroutine;
        
        private readonly float _reductionDuration = 1f;
        
        private int _targetFrameRate;
        
        private bool _isSubscribed = false;
        
        private void OnEnable() => Subscribe();
        
        private void OnDisable() => UnSubscribe();

        private void Start() => SetDefaultFrameRate();
        
        private void BoostFrameRate() { }

        private void SetDefaultFrameRate() { }

        private void ReduceFrameRate() { }
        
        private void StartFrameRateReduction(int target)
        {
            if (_reductionCoroutine != null)
                StopCoroutine(_reductionCoroutine);

            _reductionCoroutine = StartCoroutine(ReduceFrameRateCoroutine(_targetFrameRate, _reductionDuration));
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
