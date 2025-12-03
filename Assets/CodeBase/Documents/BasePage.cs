using System;
using System.Collections.Generic;
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
        
        public PageID PageIdentifier { get; set; }
        
        protected Dictionary<PageID, Func<IPage>> PageRecipes = new();
        protected Dictionary<PageID, IPage> ActivePages = new();

        protected IDocument ParentDocument;


        public BasePage(IDocument document)
        {
            ParentDocument = document;
        }

      

        public virtual void Open(VisualElement root, IDocument document)
        {
            Root = root ?? throw new System.ArgumentNullException(nameof(root));
            
            ParentDocument = document;
                
            Build();
        }

        public virtual void Close()
        {
            Logger.Log( $"Implement close page logic here....." );

            if (PageRoot == null)
            {
                Logger.Log( $"No page to close..." );

                return;
            }
            
            Root.Remove(PageRoot);
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