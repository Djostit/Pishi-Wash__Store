namespace Pishi_Wash__Store.ViewModels
{
    public class BrowseProductViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<string> Sorts { get; set; } = new() { "По возрастанию", "По убыванию" };
        public List<string> Filters { get; set; } = new() { "Все диапазоны", "0-9.99%", "10-14.99%", "15% и более"};
        public List<Product> Products { get; set; }
        public string FullName { get; set; } = Global.CurrentUser == null ? "Гость" : $"{Global.CurrentUser.UserSurname} {Global.CurrentUser.UserName} {Global.CurrentUser.UserPatronymic}";
        public string SelectedSort
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: OnSortChanged); }
        }
        void OnSortChanged()
        {
            switch (SelectedSort) 
            {
                case "По возрастанию":
                    Products = Products.OrderBy(p => p.Price).ToList();
                    break;
                case "По убыванию":
                    Products = Products.OrderByDescending(p => p.Price).ToList();
                    break;
            }
        }
        public string SelectedFilter
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: OnFilterChanged); }
        }
        async void OnFilterChanged()
        {
            var actualProduct = await _productService.GetProducts();
            switch (SelectedFilter) 
            {
                case "Все диапазоны":
                    Products = actualProduct;
                    break;
                case "0-9.99%":
                    Products = actualProduct.Where(p => p.Discount < 9).ToList();
                    break;
                case "10-14.99%":
                    Products = actualProduct.Where(p => p.Discount < 10 && p.Discount > 9).ToList();
                    break;
                case "15% и более":
                    Products = actualProduct.Where(p => p.Discount < 15 && p.Discount > 14).ToList();
                    break;
            }
        }
        public string Search
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: OnSearchChanged); }
        }

        async void OnSearchChanged()
        {
            var actualProduct = await _productService.GetProducts();
            await Task.Delay(1000);
            await Task.Run(() => 
            {
                if (string.IsNullOrEmpty(Search))
                    Products = actualProduct;
                else
                    Products = actualProduct.Where(p => p.Title.ToLower().Contains(Search.ToLower())).ToList();

                if(!string.IsNullOrEmpty(SelectedSort))
                    OnSortChanged();
            });
        }
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
