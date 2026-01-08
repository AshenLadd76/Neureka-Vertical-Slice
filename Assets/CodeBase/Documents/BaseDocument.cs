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
    public class BaseDocument : IDocument
    {
        protected VisualElement Root;
        
        protected VisualElement DocumentRoot;

        protected DocumentID DocumentID;
        
         protected readonly Dictionary<PageID, IPage> ActivePages = new();
        //protected readonly Dictionary<PageID, Func<IPage>> PageRecipes = new();
       // protected Dictionary<PageID, IPage> ActivePages = new();


        private bool _isBuilt = false;
        
        protected bool ShouldCache = false;

        
        public virtual void Open(VisualElement root)
        {
            Root = root ?? throw new System.ArgumentNullException(nameof(root));
            
            Logger.Log( $"Open Document : {Root.name}" );
            
           // Root.style.display = DisplayStyle.Flex;
        }

        public virtual void Close()
        {
            Logger.Log( $"Close Document : {Root.name} that means Document root is getting nulled which is what we dont want at all..." );
            
            //
            //if (DocumentRoot == null) return;
            //
           // Root.Remove( DocumentRoot );
            
            Root.style.display = DisplayStyle.None;
        
        }

        public virtual void Build(VisualElement root)
        {
            Logger.Log( $"BaseDocument.Build() starting..." );
            
            Root = root ?? throw new System.ArgumentNullException(nameof(root));
            
            SetDocumentRoot();
        }

        private void SetDocumentRoot()
        {
            DocumentRoot = new ContainerBuilder().AddClass(UiStyleClassDefinitions.DocumentRoot).AttachTo(Root).Build();
        }
        
        public void OpenPage(PageID id)
        {
            if (ActivePages.IsNullOrEmpty())
                return;
            
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