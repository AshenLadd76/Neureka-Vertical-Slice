using System;
using ToolBox.Messenger;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Services
{
    public class DataUploadService : MonoBehaviour
    {
        public const string DataUploadRequestMessage = "DataUploadRequest";
        
        private bool _isSubscribed = false;

        private void OnEnable() => Subscribe();
        
        private void OnDisable() => Unsubscribe();
        
        
        private void Subscribe()
        {
            if (_isSubscribed) return;
            
            MessageBus.Instance.AddListener<WebData>(NeurekaDemoMessages.DataUploadRequestMessage, HandleUploadDataRequest);
            
            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;
            
            MessageBus.Instance.RemoveListener<WebData>(NeurekaDemoMessages.DataUploadRequestMessage, HandleUploadDataRequest);
            
            _isSubscribed = false;
        }
        
        private void HandleUploadDataRequest(WebData data)
        {
            Logger.Log( $"Data upload request: I am about to save your data {  data.Id } { data.Data } to file and then I will attempt to upload it" );
           
            //TODO Validate data
           
            //TODO Save the data using file service
           
            //Attempt Upload using web service
           
            //if successful upload delete saved data
           
            //if unsuccessful keep saved file and try on app launch
          
        }
    }

    [Serializable]
    public class WebData
    {
        public string Id { get; set; }
        public string Data { get; set; }
    }
}
