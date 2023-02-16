namespace Pishi_Wash__Store.ViewModels
{
    public class BrowseProductViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public static List<Product> Products { get; set; }
        public string FullName { get; set; } = Global.CurrentUser == null ? "Гость" : $"{Global.CurrentUser.UserSurname} {Global.CurrentUser.UserName} {Global.CurrentUser.UserPatronymic}";
        public BrowseProductViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            Task.Run(async () => Products = await _productService.GetProducts())
                                 .WaitAsync(TimeSpan.FromMilliseconds(10))
                                 .ConfigureAwait(false);
        }

        public DelegateCommand SignOutCommand => new(() => 
        { 
            Global.CurrentUser = null; 
            _pageService.ChangePage(new SingInPage()); 
        });
    }
}
