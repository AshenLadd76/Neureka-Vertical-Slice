using CodeBase.Documents.Neureka.Pages;
using UnityEngine.UIElements;

namespace CodeBase.Documents.Neureka
{
    public class NeurekaDocument : BaseDocument
    {
        private bool _isBuilt;
        
        private void AddPageRecipes()
        {
            //PageRecipes[PageID.Splash] = () => new SplashPage(this);
           // PageRecipes[PageID.NavPage] = () => new NavPage(this);
           // PageRecipes[PageID.InfoPage] = () => new InfoPage(this);
            
            PageRecipes[PageID.Splash] =  new SplashPage(this);
            PageRecipes[PageID.NavPage] =  new NavPage(this);
            PageRecipes[PageID.InfoPage] =  new InfoPage(this);
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
