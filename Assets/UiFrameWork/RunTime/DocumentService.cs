using System;
using System.Collections.Generic;
using CodeBase.Documents;
using CodeBase.Documents.DemoA;
using CodeBase.Documents.Neureka;
using CodeBase.Documents.Neureka.Assessments;
using CodeBase.Pages;
using ToolBox.Messenger;
using ToolBox.Utils.Validation;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace UiFrameWork.RunTime
{
    [RequireComponent(typeof(UIDocument))]
    public class DocumentService : MonoBehaviour
    {
        private Dictionary<DocumentID, Func<IDocument>> _documents;

        private Dictionary<DocumentID, IDocument> _activeDocuments;
        
        private UIDocument _uiDocument;
        
        private VisualElement _rootVisualElement;

        private bool _isSubscribed = false;
        
        private void OnEnable()
        {
            if (_isSubscribed) return;
            
            MessageBus.Instance.AddListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestOpenDocument), OnRequestOpenDocument );
            MessageBus.Instance.AddListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestCloseDocument), OnRequestCloseDocument  );
            
            _isSubscribed = true;
        }

        private void OnDisable()
        {
            if( !_isSubscribed) return;
            
            MessageBus.Instance.RemoveListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestOpenDocument), OnRequestOpenDocument );
            MessageBus.Instance.RemoveListener<DocumentID>( nameof(DocumentServiceMessages.OnRequestCloseDocument), OnRequestCloseDocument);
            
            _isSubscribed = false;
        }
        
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            ObjectValidator.Validate(_uiDocument);
            
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            InitRecipeDictionary();
            
            _activeDocuments = new Dictionary<DocumentID, IDocument>();
        }

        private void InitRecipeDictionary()
        {
            _documents = new Dictionary<DocumentID, Func<IDocument>>
            {
                [DocumentID.Nerueka] = () => new NeurekaDocument(),
                [DocumentID.Hub] = () => new HubDocument(),
                [DocumentID.TestDocument] = () => new TestDocument(),
                [DocumentID.RiskFactors] = () => new RiskFactorsDocument()
              
            };
        }

        private void OnRequestOpenDocument(DocumentID documentID)
        {
            if (_activeDocuments.ContainsKey(documentID))
            {
                Logger.Log($"Document {documentID} is already open.");
                return;
            }
            
            if (!_documents.TryGetValue(documentID, out Func<IDocument> documentFunc))
            {
                Logger.LogError($"Document {documentID} not found");
                return;
            }
            
            IDocument document = documentFunc();
            
            _activeDocuments.Add( documentID, document );
            
            document.Open(_rootVisualElement);
        }

        private void OnRequestCloseDocument(DocumentID documentID)
        {
            Logger.Log($"Request to close Page {documentID}");

            if (_activeDocuments.ContainsKey(documentID))
            {
                _activeDocuments.Remove(documentID);
            }
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

