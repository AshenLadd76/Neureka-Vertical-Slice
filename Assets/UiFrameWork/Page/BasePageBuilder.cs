using CodeBase.Documents;
using CodeBase.UiComponents.Headers;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;
using UiFrameWork.RunTime;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace UiFrameWork.Page
{
    public class BasePageBuilder : IPage
    {
        private VisualElement _root;

        private VisualElement _pageRoot;

        public PageID PageIdentifier { get; set; }

        public void Open(VisualElement root, IDocument document)
        {
            _root = root;

            BuildPageRoot();
        }
        
        public void Close()
        {
            throw new System.NotImplementedException();
        }


        private void BuildPageRoot()
        {
            _pageRoot = new ContainerBuilder()
                .AddClass(UiStyleClassDefinitions.PageRoot)
                .AttachTo(_root)
                .Build();
        }

        protected virtual BasePageBuilder AddHeader()
        {
            new DefaultHeader("Main Hub", _pageRoot, 
                () => { Logger.Log($"OnBack Selected"); },
                Close);
            
            return this;
        }

        protected virtual BasePageBuilder AddContent()
        {
            return this;
        }

        protected virtual BasePageBuilder AddFooter()
        {
            return this;
        }

        public void Build()
        {
            
        }
    }
}
