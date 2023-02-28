namespace Pishi_Wash__Store.ViewModels
{
    public class BrowseProductViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<string> Sorts { get; set; } = new() { "По возрастанию", "По убыванию" };
        public List<string> Filters { get; set; } = new() { "Все диапазоны", "0-9.99%", "10-14.99%", "15% и более" };
        public List<Product> Products { get; set; }
        public string FullName { get; set; } = UserSetting.Default.UserName == string.Empty ? "Гость" : $"{UserSetting.Default.UserSurname} {UserSetting.Default.UserName} {UserSetting.Default.UserPatronymic}";
        public int? MaxRecords { get; set; } = 0;
        public int? Records { get; set; } = 0;

        public string SelectedSort
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }
        public string SelectedFilter
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }
        public string Search
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }

        private async void UpdateProduct()
        {
            var currentProduct = await _productService.GetProducts();
            MaxRecords = currentProduct.Count;

            if (!string.IsNullOrEmpty(SelectedFilter))
            {
                switch (SelectedFilter)
                {
                    case "0-9.99%":
                        currentProduct = currentProduct.Where(p => p.Discount > 0 && p.Discount < 10).ToList();
                        break;
                    case "10-14.99%":
                        currentProduct = currentProduct.Where(p => p.Discount > 10 && p.Discount < 15).ToList();
                        break;
                    case "15% и более":
                        currentProduct = currentProduct.Where(p => p.Discount > 15).ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(Search))
                currentProduct = currentProduct.Where(p => p.Title.ToLower().Contains(Search.ToLower())).ToList();

            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По возрастанию":
                        currentProduct = currentProduct.OrderBy(p => p.Price).ToList();
                        break;
                    case "По убыванию":
                        currentProduct = currentProduct.OrderByDescending(p => p.Price).ToList();
                        break;
                }
            }

            Records = currentProduct.Count;
            Products = currentProduct;
        }
        public BrowseProductViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            Task.Run(async () =>
            {
                Products = await _productService.GetProducts();
                MaxRecords = Products.Count;
                Records = MaxRecords;
            }).WaitAsync(TimeSpan.FromSeconds(1))
            .ConfigureAwait(false);
            SelectedFilter = "Все диапазоны";
        }

        public DelegateCommand SignOutCommand => new(() =>
        {
            UserSetting.Default.Id = 0;
            UserSetting.Default.UserName = string.Empty;
            UserSetting.Default.UserSurname = string.Empty;
            UserSetting.Default.UserPatronymic = string.Empty;
            UserSetting.Default.UserRole = 0;
            _pageService.ChangePage(new SingInPage());
        });
    }
}
