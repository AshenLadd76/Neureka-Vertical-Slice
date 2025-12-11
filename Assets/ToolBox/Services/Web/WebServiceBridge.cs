using System;
using System.Collections;
using System.IO;
using CodeBase.Services;
using CodeBase.Services.Encryption;
using ToolBox.Helpers;
using ToolBox.Messenger;
using ToolBox.Services.Auth;
using ToolBox.Services.Data;
using ToolBox.Services.Encryption;
using ToolBox.Utils.Validation;
using UnityEngine;
using UnityEngine.Events;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Services.Web
{
    public class WebServiceBridge : BaseService
    { 
        private const string ServerUrl = "http://localhost:5000/api/data";
        
        private ISerializer _serializer;
        private IEncryptionService _encryptionService;
        private ITokenService _tokenService;
        private ICoroutineRunner _coroutineRunner;
        
        private IWebService _webService; 
        private IFileDataService _fileDataService;

        private const string DataDirectory = "Data";
        private const string FileExtenstion = ".json";
       
        [SerializeField] private UnityEvent<string> onServerResponse;
       
        [Validate] private MessageBus _messageBus;

         private void Awake()
         {
             InitServices();
           
            ObjectValidator.Validate(this, null, true);
         }
       
       
        private void InitServices()
        { 
            _messageBus = MessageBus.Instance;
            _serializer = new JsonSerializer();
            _encryptionService = new EncryptionService();
            _tokenService = new TokenService(_encryptionService);
            _coroutineRunner = new CoroutineRunner(this);
            _fileDataService = new FileDataService(_encryptionService, _serializer, FileExtenstion, GetDataPath() );
            _webService = new WebService(_serializer, _tokenService, _coroutineRunner);
        }

        
        private void OnPostRequest(WebData webData)
        {
            Logger.Log( $"Request to post data received: {webData.Id} {webData.Data}" );
            
            //1. Validate incoming data
            if (webData == null || string.IsNullOrEmpty(webData.Data) || string.IsNullOrEmpty(webData.Id))
            {
                Logger.LogError( $"Web Service Request Failed: web data is null or empty." );
                return;
            }
            
            //2. save the data
            SaveWebData(webData);
            
            //3. Create FormData object
            var formData =  new FormData( webData.Id, ServerUrl, webData.Data  );
            
            //4. Start the request
            StartCoroutine( HandleWebRequest(_webService.Post, formData ));
        }

        private IEnumerator HandleWebRequest(Func<IFormData, IEnumerator> requestMethod, IFormData formData)
        {
            if (requestMethod == null || formData == null)
            {
                Logger.LogError("Request method or data is null!");
                yield break;
            }
            
            yield return requestMethod(formData);

            HandleServerResponse( formData.Title );
        }

        private void HandleServerResponse(string title)
        {
            var success = _webService.GetResponseStatus();
            
            if (success)
            {
                Logger.Log($"Request for {title} succeeded!");
          
                string fileToDelete = $"{title}.json";
                
                _fileDataService.Delete("Data", fileToDelete);
            }
            else
            {
                Logger.LogError($"Request for {title} failed!");
                // You could leave the file for retry
            }
        }


        private void SaveWebData(WebData webData)
        {
            var data = webData.Data;
            
            string fileToSave = $"{webData.Id}{FileExtenstion}";
            
            //1. save the data
            var result = _fileDataService.Save(data, DataDirectory, fileToSave, false, true );
            
            //2. check save status
            if (result.Success)
            {
                Logger.Log($"Successfully saved {fileToSave}");
            }
            else
            {
                Logger.LogError($"Failed to save file {fileToSave}");
            }
        }
        
        private string GetDataPath() => Path.Combine(Application.persistentDataPath, DataDirectory);


        protected override void SubscribeToService()
        {
            _messageBus.AddListener<WebData>( WebServiceMessages.OnPostRequestMessage, OnPostRequest );
        }

        protected override void UnsubscribeFromService()
        {
            _messageBus.RemoveListener<WebData>( WebServiceMessages.OnPostRequestMessage, OnPostRequest );
        }
        
    }

    public static class WebServiceMessages
    {
        public const string OnPostRequestMessage = "OnPostRequest";
    }
    
}
