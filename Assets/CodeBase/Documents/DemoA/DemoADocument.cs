using CodeBase.Documents.DemoA.Pages;

namespace CodeBase.Documents.DemoA
{
    public class DemoADocument : BaseDocument
    {
        private void AddPageRecipes()
        {
            PageRecipes[PageID.Splash] = () => new SplashPage(this);
            PageRecipes[PageID.NavPage] = () => new NavPage(this);
            PageRecipes[PageID.InfoPage] = () => new InfoPage(this);
        }
        
        protected override void Build()
        {
            base.Build();
            
            //Add page recipes here
            AddPageRecipes();
            
            OpenPage(PageID.Splash);
        }
    }
}
