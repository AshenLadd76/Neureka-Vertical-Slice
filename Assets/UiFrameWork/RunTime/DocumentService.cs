using System;
using System.Collections.Generic;
using CodeBase;
using CodeBase.Documents;
using CodeBase.Documents.Neureka;
using CodeBase.Documents.Neureka.Assessments.RiskFactors;
using CodeBase.Documents.Neureka.Navigation;
using CodeBase.Pages;
using ToolBox.Helpers;
using ToolBox.Messaging;
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
        private Dictionary<DocumentID, Func<IDocument>> _documents;

        private Dictionary<DocumentID, IDocument> _cachedDocuments;
        
        [Validate] private UiDocumentManager _uiDocumentManager;
        
        [Validate] private VisualElement _safeAreaContainer;
        
        
        
        [Validate] private IFileDataService _fileDataService;
        
        [Validate] private ICoroutineRunner _coroutineRunner;
        
        private const string DefaultFileExtension = ".json";

        private void Awake() => Init();

        private void Init()
        {
            InitUi();
            
            InitFileDataService();

            _coroutineRunner = new CoroutineRunner(this);
            
            InitRecipeDictionaries();
            
        }

        private void InitUi()
        {
            _uiDocumentManager = GetComponent<UiDocumentManager>();

            _safeAreaContainer = _uiDocumentManager.SafeAreaContainer;

        }
        
        private void InitFileDataService() => _fileDataService = new FileDataService(new EncryptionService(), new JsonSerializer(), DefaultFileExtension);

        private void InitRecipeDictionaries()
        {
            _documents = new Dictionary<DocumentID, Func<IDocument>>
            {
                [DocumentID.Splash] = () => new SplashDocument(_coroutineRunner),
                [DocumentID.Nav] = () => new NavDocument(),
                [DocumentID.Neureka] = () => new NeurekaDocument(_coroutineRunner),
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
                _cachedDocuments[documentID].Open(_safeAreaContainer);
                return;
            }
            
            if (!_documents.TryGetValue(documentID, out Func<IDocument> documentFunc))
            {
                Logger.LogError($"Document {documentID} not found");
                return;
            }
            
            IDocument document = documentFunc();

            if (document.ShouldCache)
            {
                Logger.Log($"Document {documentID} cached");
                _cachedDocuments[documentID] = document;
            }

            document.Build(_safeAreaContainer);
            document.Open(_safeAreaContainer);
        }
        
        protected override void SubscribeToService() => MessageBus.AddListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestOpenDocument), OnRequestOpenDocument );
        
        protected override void UnsubscribeFromService() => MessageBus.RemoveListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestOpenDocument), OnRequestOpenDocument );
    }

    public enum DocumentID
    { 
        Splash,
        Nav,
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

