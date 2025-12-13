using System;
using ToolBox.Messaging;
using UnityEngine;
using UnityEngine.Events;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Services
{
    public class DataUploadService : MonoBehaviour
    {
        public const string DataUploadRequestMessage = "DataUploadRequest";
        
        private bool _isSubscribed = false;

        private void OnEnable() => Subscribe();
        
        private void OnDisable() => Unsubscribe();
        
        [SerializeField] private UnityEvent <WebData> onUploadRequestReceived = null;
        [SerializeField] private UnityEvent<string> onSuccessfulUpload;
        
        private WebData _webData;
        
        
        private void Subscribe()
        {
            if (_isSubscribed) return;
            
            MessageBus.AddListener<WebData>(NeurekaDemoMessages.DataUploadRequestMessage, HandleUploadDataRequest);
            
            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;
            
            MessageBus.RemoveListener<WebData>(NeurekaDemoMessages.DataUploadRequestMessage, HandleUploadDataRequest);
            
            _isSubscribed = false;
        }
        
        private void HandleUploadDataRequest(WebData data)
        {
            Logger.Log( $"Data upload request: I am about to save your data {  data.Id } { data.Data } to file and then I will attempt to upload it" );
            
            _webData = data;
            
            onUploadRequestReceived.Invoke( _webData );
           
            //TODO Validate data
            
            //TODO Save the data using file service
           
            //Attempt Upload using web service
           
            //if successful upload delete saved data
           
            //if unsuccessful keep saved file and try on app launch
        }
        
        
        public void OnServerResponse(string response)
        {
            if (response == "200")
            {
                Logger.Log( $"Data upload was successful. So im going to delete the temp saved data now." );
                onSuccessfulUpload?.Invoke( _webData.Id );
            }
            else
            {
                Logger.Log($"Data upload failed, so i will keep the saved data and try again later...");
            }
                
                
        }
    }

    [Serializable]
    public class WebData
    {
        public string Id { get; set; }
        public string Data { get; set; }
    }
}
