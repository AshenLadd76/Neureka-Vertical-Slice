using UnityEngine;
using UnityEngine.Events;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Services
{
    public class WebService : MonoBehaviour
    {
        private const string ServerUrl = "https://localhost:5000/Questionnaires/";

        private const string ServerErrorCode = "500";
        private const string ServerSuccessResponse = "200";

        [SerializeField] private UnityEvent<string> onServerResponse;
        
        public void DataUploadRequest(WebData data)
        {
            Logger.Log( $"WebService Here.. I have received a request to upload data to >>> {ServerUrl}" );
            
            Invoke( nameof( HandleServerResponse ), Random.Range( 1, 6 ) );
        }

        public void HandleServerResponse()
        {
            onServerResponse?.Invoke(Random.value < 0.5f ? ServerErrorCode : ServerSuccessResponse);
        }
    }
}
