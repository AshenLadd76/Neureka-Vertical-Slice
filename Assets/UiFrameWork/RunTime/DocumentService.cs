using System;
using System.Collections.Generic;
using CodeBase.Documents;
using CodeBase.Documents.Neureka;
using CodeBase.Documents.Neureka.Assessments.RiskFactors;
using CodeBase.Pages;
using ToolBox.Helpers;
using ToolBox.Messenger;
using ToolBox.Services;
using ToolBox.Services.Data;
using ToolBox.Services.Encryption;
using ToolBox.Utils.Validation;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace UiFrameWork.RunTime
{
    [RequireComponent(typeof(UIDocument))]
    public class DocumentService : BaseService
    {
        [Validate] private Dictionary<DocumentID, Func<IDocument>> _documents;

        [Validate] private Dictionary<DocumentID, IDocument> _cachedDocuments;
        
        [Validate] private UIDocument _uiDocument;
        
        [Validate] private VisualElement _rootVisualElement;
        
        [Validate] private IFileDataService _fileDataService;
        
        [Validate] private MessageBus _messageBus;
        
        private const string DefaultFileExtension = ".json";

        private void Awake() => Init();

        private void Init()
        {
            InitUi();
            
            InitFileDataService();
            
            InitRecipeDictionaries();
            
            _messageBus = MessageBus.Instance;
            
            ObjectValidator.Validate( _messageBus );
        }

        private void InitUi()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            _rootVisualElement = _uiDocument.rootVisualElement;
        }
        
        private void InitFileDataService() => _fileDataService = new FileDataService(new EncryptionService(), new JsonSerializer(), DefaultFileExtension);

        private void InitRecipeDictionaries()
        {
            _documents = new Dictionary<DocumentID, Func<IDocument>>
            {
                [DocumentID.Neureka] = () => new NeurekaDocument(),
                [DocumentID.Hub] = () => new HubDocument(),
                [DocumentID.TestDocument] = () => new TestDocument(),
                [DocumentID.RiskFactors] = () => new RiskFactorsDocument(_fileDataService)
            };
            
            _cachedDocuments = new Dictionary<DocumentID, IDocument>();
        }

        private void OnRequestOpenDocument(DocumentID documentID)
        {
            if (_cachedDocuments.TryGetValue(documentID, out var activeDocument))
            {
                TryOpenDocument(activeDocument);
                return;
            }
            
            if (!_documents.TryGetValue(documentID, out Func<IDocument> documentFunc))
            {
                Logger.LogError($"Document {documentID} not found");
                return;
            }
            
            IDocument document = documentFunc();
            
            _cachedDocuments[documentID] = document;

            if (TryOpenDocument(document)) return;
            
            Logger.LogError($"Failed to open document {documentID}, retry may fail.");
   
            _cachedDocuments.Remove(documentID);
        }

        private void OnRequestCloseDocument(DocumentID documentID) => Logger.Log($"Document {documentID} has been closed");
        
        
        /// <summary>
        /// Tries to open a document and logs any errors that occur.
        /// Returns true if successful, false otherwise.
        /// </summary>
        private bool TryOpenDocument(IDocument document)
        {
            try
            {
                document.Open(_rootVisualElement);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to open document {document.GetType().Name}: {ex.Message}");
                return false;
            }
        }
        
        protected override void SubscribeToService()
        {
            _messageBus.AddListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestOpenDocument), OnRequestOpenDocument );
            //MessageBus.Instance.AddListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestCloseDocument), OnRequestCloseDocument  );
        }

        protected override void UnsubscribeFromService()
        {
            _messageBus.RemoveListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestOpenDocument), OnRequestOpenDocument );
           // MessageBus.Instance.RemoveListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestCloseDocument), OnRequestCloseDocument);
        }
    }

    public enum DocumentID
    { 
        Hub,
        Neureka,
        TestDocument,
        RiskFactors,
    }

    public enum DocumentServiceMessages
    {
        OnRequestOpenDocument,
        OnRequestCloseDocument
    }
}

