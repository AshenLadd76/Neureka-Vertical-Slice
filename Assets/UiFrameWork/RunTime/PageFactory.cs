using System;
using System.Collections.Generic;
using CodeBase.Pages;
using ToolBox.Messenger;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace UiFrameWork.RunTime
{
    [RequireComponent(typeof(UIDocument))]
    public class PageFactory : MonoBehaviour
    {
        private Dictionary<PageID, Func<IPage>> _pages;
        
        private UIDocument _uiDocument;
        
        private VisualElement _rootVisualElement;
        
        private void OnEnable()
        {
            MessageBus.Instance.AddListener<PageID>( nameof(PageFactoryMessages.OnRequestPage), OnRequestPage );
        }

        private void OnDisable()
        {
            MessageBus.Instance.RemoveListener<PageID>( nameof(PageFactoryMessages.OnRequestPage), OnRequestPage );
        }
        

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            InitRecipeDictionary();
        }

        private void InitRecipeDictionary()
        {
            _pages = new Dictionary<PageID, Func<IPage>>
            {
                [PageID.TestPage] = () => new TestPage()
            };
        }

        private void OnRequestPage(PageID pageID)
        {
            if (!_pages.TryGetValue(pageID, out Func<IPage> pageFunc))
            {
                Logger.LogError($"Page {pageID} not found");
                return;
            }
            
            IPage page = pageFunc();
            
            page.Open(_rootVisualElement);
            
        }

     
       
    }

    public enum PageID
    {
        TestPage,
    }

    public enum PageFactoryMessages
    {
        OnRequestPage
    }
}

