using CodeBase.Documents.DemoA.Pages;
using CodeBase.Pages;
using CodeBase.UiComponents.Styles;
using UiFrameWork.Builders;

namespace CodeBase.Documents.DemoA
{
    public class DemoADocument : BaseDocument
    {
        
        private void AddPageRecipes()
        {
            PageRecipes[PageID.Splash] = () => new SplashPage();
            PageRecipes[PageID.InfoPage] = () => new InfoPage();
        }
        
        protected override void Build()
        {
            base.Build();
            
            //Add page recipes here
            AddPageRecipes();
            
            OpenPage(PageID.InfoPage);
        }
    }
}
