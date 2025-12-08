using System;
using System.Collections.Generic;
using CodeBase.Documents;
using CodeBase.Documents.Neureka;
using CodeBase.Documents.Neureka.Assessments;
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
        
        private const string DefaultFileExtension = ".json";

        private void Awake() => Init();

        private void Init()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            _fileDataService = new FileDataService(new EncryptionService(), new JsonSerializer(), DefaultFileExtension);
            
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            _cachedDocuments = new Dictionary<DocumentID, IDocument>();
            
            InitRecipeDictionary();
            
            ObjectValidator.Validate(_uiDocument);
        }

        private void InitRecipeDictionary()
        {
            _documents = new Dictionary<DocumentID, Func<IDocument>>
            {
                [DocumentID.Nerueka] = () => new NeurekaDocument(),
                [DocumentID.Hub] = () => new HubDocument(),
                [DocumentID.TestDocument] = () => new TestDocument(),
                [DocumentID.RiskFactors] = () => new RiskFactorsDocument(_fileDataService)
            };
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
            
            if( TryOpenDocument(document) )
                _cachedDocuments.Add( documentID, document );
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
            MessageBus.Instance.AddListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestOpenDocument), OnRequestOpenDocument );
            //MessageBus.Instance.AddListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestCloseDocument), OnRequestCloseDocument  );
        }

        protected override void UnsubscribeFromService()
        {
            MessageBus.Instance.RemoveListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestOpenDocument), OnRequestOpenDocument );
           // MessageBus.Instance.RemoveListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestCloseDocument), OnRequestCloseDocument);
        }
    }

    public enum DocumentID
    { 
        Hub,
        Nerueka,
        TestDocument,
        RiskFactors,
    }

    public enum DocumentServiceMessages
    {
        OnRequestOpenDocument,
        OnRequestCloseDocument
    }
}

