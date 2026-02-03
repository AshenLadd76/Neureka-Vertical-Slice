using System.Collections.Generic;
using CodeBase.UiComponents.Styles;
using ToolBox.Extensions;
using UiFrameWork.Components;
using UiFrameWork.Page;
using UiFrameWork.RunTime;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents
{
    /// <summary>
    /// Base class for all documents in the application.
    /// Handles opening, closing, building, and managing pages within a document.
    /// </summary>
    
    public class BaseDocument : IDocument
    {
        public virtual bool ShouldCache => false;

        protected VisualElement Root;

        public VisualElement DocumentRoot { get; set; }

        protected DocumentID DocumentID;
        
        protected readonly Dictionary<PageID, IPage> ActivePages = new();
        
        
        public virtual void Open(VisualElement root)
        {
            if (DocumentRoot != null) DocumentRoot.style.display = DisplayStyle.Flex;
        }

        public virtual void Close()
        {
            if (DocumentRoot == null)
            {
                return;
            }

            if (ShouldCache)
                DocumentRoot.style.display = DisplayStyle.None;
            else
                DocumentRoot.RemoveFromHierarchy();
        }

        public virtual void Build(VisualElement root)
        {
            DocumentRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.DocumentRoot).AttachTo(root).Build();
        }

        private void SetDocumentRoot()
        {
            DocumentRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.DocumentRoot).AttachTo(Root).Build();
        }
        
        public void OpenPage(PageID id)
        {
            if (ActivePages.IsNullOrEmpty())
            {
                Logger.Log( $"OpenPage() starting...Acitve pages are empty" );
                return;
            }

            var pageToOpen = ActivePages[id];
            
            pageToOpen.PageIdentifier = id;
            
            SetDocumentRoot();
            
            pageToOpen.Open(DocumentRoot, this);
        }

        public void ClosePage(PageID id, VisualElement page)
        {
            //RemoveActivePage(id);
            
            ActivePages.Remove(id);
            
            RemovePageFromDocument(page);
        }

        private void RemovePageFromDocument(VisualElement page)
        {
            if(page == null) return;

            if (DocumentRoot != page.parent) return;
            
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