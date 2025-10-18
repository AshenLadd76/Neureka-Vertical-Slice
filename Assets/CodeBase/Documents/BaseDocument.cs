using System;
using System.Collections.Generic;

using CodeBase.UiComponents.Styles;
using ToolBox.Messenger;
using UiFrameWork.Builders;
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
        
        protected Dictionary<PageID, Func<IPage>> PageRecipes = new();
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
        
        protected void OpenPage(PageID id)
        {
            if (PageRecipes.TryGetValue(id, out var recipe))
            {
                Logger.Log( $"BaseDocument.OpenPage() starting... id: {id}" );
                recipe()?.Open(DocumentRoot);
            }
            else
            {
                Logger.LogError( $"BaseDocument.OpenPage() page not found... id: {id}" );
            }
        }
    }
}