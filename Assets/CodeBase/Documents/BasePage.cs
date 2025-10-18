using System;
using System.Collections.Generic;
using CodeBase.Pages;
using ToolBox.Messenger;
using UiFrameWork.Page;
using UiFrameWork.RunTime;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents
{
    public class BasePage : IPage
    {
        protected VisualElement Root;
        
        protected VisualElement PageRoot;

        protected PageID PageIdentifier;
        
        protected Dictionary<PageID, Func<IPage>> PageRecipes = new();
        protected Dictionary<PageID, IPage> ActivePages = new();
       
        
        public virtual void Open(VisualElement root)
        {
            Root = root ?? throw new System.ArgumentNullException(nameof(root));
                
            Build();
        }

        public virtual void Close()
        {
            if (PageRoot == null) return;
            
            MessageBus.Instance.Broadcast(nameof(DocumentFactoryMessages.OnRequestCloseDocument), PageIdentifier);
            
            Root?.Remove( PageRoot ); PageRoot = null;
        }

        protected virtual void Build()
        {
            Logger.Log( $"PageDocument.Build() starting..." );
            
            if (Root == null)
            {
                Logger.LogError("Root page has not been initialized.");
                return;
            }
        }
    }
}