namespace Pishi_Wash__Store.ViewModels
{
    public class CartViewModel : BindableBase
    {
        private readonly PageService _pageService;
        public CartViewModel(PageService pageService)
        {
            _pageService = pageService;
        }
        public DelegateCommand ReturnBackCommand => new(() =>
        {
            _pageService.ChangePage(new BrowseProductPage());
        });
        public DelegateCommand SignOutCommand => new(() =>
        {
            UserSetting.Default.Id = 0;
            UserSetting.Default.UserName = string.Empty;
            UserSetting.Default.UserSurname = string.Empty;
            UserSetting.Default.UserPatronymic = string.Empty;
            UserSetting.Default.UserRole = 0;
            Global.CurrentCart.Clear();
            _pageService.ChangePage(new SingInPage());
        });
    }
}
