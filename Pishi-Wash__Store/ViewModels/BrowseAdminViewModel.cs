namespace Pishi_Wash__Store.ViewModels
{
    public class BrowseAdminViewModel : BindableBase
    {
        private readonly PageService _pageService;
        public BrowseAdminViewModel(PageService pageService)
        {
            _pageService = pageService;
        }
    }
}
