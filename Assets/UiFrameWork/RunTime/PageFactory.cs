using System;
using System.Collections.Generic;
using CodeBase.Pages;
using ToolBox.Messenger;
using ToolBox.Utils.Validation;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace UiFrameWork.RunTime
{
    [RequireComponent(typeof(UIDocument))]
    public class PageFactory : MonoBehaviour
    {
        private Dictionary<PageID, Func<IPage>> _pages;

        private Dictionary<PageID, IPage> _activePages;
        
        private UIDocument _uiDocument;
        
        private VisualElement _rootVisualElement;

        private bool _isSubscribed = false;
        
        private void OnEnable()
        {
            if (_isSubscribed) return;
            
            MessageBus.Instance.AddListener<PageID>( nameof(PageFactoryMessages.OnRequestOpenPage), OnRequestOpenPage );
            MessageBus.Instance.AddListener<PageID>( nameof(PageFactoryMessages.OnRequestClosePage), OnRequestClosePage  );
            
            _isSubscribed = true;
        }

        private void OnDisable()
        {
            if( !_isSubscribed) return;
            
            MessageBus.Instance.RemoveListener<PageID>( nameof(PageFactoryMessages.OnRequestOpenPage), OnRequestOpenPage );
            MessageBus.Instance.RemoveListener<PageID>( nameof(PageFactoryMessages.OnRequestClosePage), OnRequestClosePage);
            
            _isSubscribed = false;
        }
        
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            ObjectValidator.Validate(_uiDocument);
            
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            InitRecipeDictionary();
            
            _activePages = new Dictionary<PageID, IPage>();
        }

        private void InitRecipeDictionary()
        {
            _pages = new Dictionary<PageID, Func<IPage>>
            {
                [PageID.TestPage] = () => new TestPage(),
                [PageID.HubPage] = () => new HubPage()
            };
        }

        private void OnRequestOpenPage(PageID pageID)
        {
            if (_activePages.ContainsKey(pageID))
            {
                Logger.Log($"Page {pageID} is already open.");
                return;
            }
            
            if (!_pages.TryGetValue(pageID, out Func<IPage> pageFunc))
            {
                Logger.LogError($"Page {pageID} not found");
                return;
            }
            
            IPage page = pageFunc();
            
            _activePages.Add( pageID, page );
            
            page.Open(_rootVisualElement);
        }

        private void OnRequestClosePage(PageID pageID)
        {
            Logger.Log($"Request to close Page {pageID}");

            if (_activePages.ContainsKey(pageID))
            {
                _activePages.Remove(pageID);
            }
        }
    }

    public enum PageID
    {
        HubPage,
        TestPage,
    }

    public enum PageFactoryMessages
    {
        OnRequestOpenPage,
        OnRequestClosePage
    }
}

