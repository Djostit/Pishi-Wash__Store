namespace Pishi_Wash__Store.ViewModels
{
    public class CartViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }
        public CartViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            Task.Run(async () => Products = await _productService.GetCart());
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
        public DelegateCommand RemoveCommand => new(() => 
        {
            
        });
    }
}
