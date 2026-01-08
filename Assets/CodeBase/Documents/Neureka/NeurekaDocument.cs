using CodeBase.Documents.Neureka.Pages;
using UnityEngine.UIElements;

namespace CodeBase.Documents.Neureka
{
    public class NeurekaDocument : BaseDocument
    {
        private bool _isBuilt;
        
        private void AddPageRecipes()
        {
            ActivePages[PageID.Splash] =  new SplashPage(this);
            ActivePages[PageID.NavPage] =  new NavPage(this);
            ActivePages[PageID.InfoPage] =  new InfoPage(this);
        }
        
        public override void Build(VisualElement root)
        {
            base.Build(root);
            
            //Add page recipes here
            AddPageRecipes();
            
            OpenPage(PageID.Splash);
        }

        public override void Open(VisualElement root)
        {
            OpenPage(PageID.NavPage);
        }
    }
}
