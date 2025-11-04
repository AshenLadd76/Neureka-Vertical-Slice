using System;
using System.Collections.Generic;

using CodeBase.UiComponents.Styles;
using ToolBox.Messenger;
using UiFrameWork.Builders;
using UiFrameWork.Components;
using UiFrameWork.Page;
using UiFrameWork.RunTime;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents
{
    public class BaseDocument : IDocument
    {
        protected VisualElement Root;
        
        protected VisualElement DocumentRoot;

        protected DocumentID DocumentID;
        
        protected readonly Dictionary<PageID, Func<IPage>> PageRecipes = new();
        protected Dictionary<PageID, IPage> ActivePages = new();
       
        
        public virtual void Open(VisualElement root)
        {
            Root = root ?? throw new System.ArgumentNullException(nameof(root));
            
            Logger.Log( $"Open Document : {Root.name}" );
                
            Build();
        }

        public virtual void Close()
        {
            if (DocumentRoot == null) return;
            
            MessageBus.Instance.Broadcast(nameof(DocumentFactoryMessages.OnRequestCloseDocument), DocumentID);
            
            Root?.Remove( DocumentRoot );
            DocumentRoot = null;
        }

        protected virtual void Build()
        {
            Logger.Log( $"BaseDocument.Build() starting..." );
            DocumentRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.DocumentRoot).AttachTo(Root).Build();
        }
        
        public void OpenPage(PageID id)
        {
            if (ActivePages.TryGetValue(id, out var page))
            {
                Logger.Log( $"Page : {id} is already loaded....opening" );
                page.Open(DocumentRoot,this);
                return;
            }
            
            if (PageRecipes.TryGetValue(id, out var recipe))
            {
                Logger.Log($"BaseDocument.OpenPage() starting... id: {id}");

                // Instantiate the page from the recipe
                var pageToOpen = recipe();

                if (pageToOpen == null)
                {
                    Logger.LogError($"Failed to create page for id: {id}");
                    return;
                }

                // Add to ActivePages
                ActivePages[id] = pageToOpen;

                // Open the page with the document's root and pass the document itself
                pageToOpen.PageIdentifier = id;
                pageToOpen.Open(DocumentRoot, this);
            }
            else
            {
                Logger.LogError($"BaseDocument.OpenPage() page not found... id: {id}");
            }
        }

        public void ClosePage(PageID id, VisualElement page)
        {
            RemoveActivePage(id);
            
            RemovePageFromDocument(page);
        }

        private void RemovePageFromDocument(VisualElement page)
        {
            if(page == null) return;
            
            DocumentRoot?.Remove(page);
        }
        
        private void RemoveActivePage(PageID pageID)
        {
            if(ActivePages.ContainsKey(pageID))
            {
                ActivePages.Remove(pageID);
                Logger.Log($"Removed page {pageID} from ActivePages");
            }
            else
            {
                Logger.LogWarning($"Page {pageID} was not in ActivePages");
            }
        }
    }
}