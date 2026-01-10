using CodeBase.Documents.Neureka.Pages;
using ToolBox.Helpers;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.Documents.Neureka
{
    public class NeurekaDocument : BaseDocument
    {
        private bool _isBuilt;

        private float _splashDelay = 3f;
        
        private ICoroutineRunner _coroutineRunner;
        private Coroutine _openPageCoroutine;

        

        public NeurekaDocument(ICoroutineRunner coroutineRunner)
        {
         
            
            _coroutineRunner = coroutineRunner;
        }
        
        private void AddPageRecipes()
        {
            ActivePages.Clear();
            
            ActivePages[PageID.Splash] =  new SplashPage(this);
            ActivePages[PageID.NavPage] =  new NavPage(this);
            ActivePages[PageID.InfoPage] =  new InfoPage(this);
        }
        
        public override void Build(VisualElement root)
        {
            base.Build(root);
            
            //Add page recipes here
            AddPageRecipes();
            OpenPage(PageID.NavPage);
            OpenPage(PageID.Splash);
        }

        public override void Open(VisualElement root)
        {
            OpenPage(PageID.NavPage);
        }

     
    }
}
