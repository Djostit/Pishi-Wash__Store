namespace Pishi_Wash__Store.ViewModels
{
    public class BrowseAdminViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<string> Sorts { get; set; } = new() { "По возрастанию", "По убыванию" };
        public List<string> Filters { get; set; } = new() { "Все диапазоны", "Не завершен", "Завершен" };
        public List<Order> Orders { get; set; }
        public Order SelectedOrder { get; set; }
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
        public BrowseAdminViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
        }
        private async void UpdateProduct()
        {
            var currentOrders = await _productService.GetOrders();
            MaxRecords = currentOrders.Count;

            if (!string.IsNullOrEmpty(SelectedFilter))
            {
                switch (SelectedFilter)
                {
                    case "Не завершен":
                        currentOrders = currentOrders.Where(c => c.OrderStatus != "Завершен").ToList();
                        break;
                    case "Завершен":
                        currentOrders = currentOrders.Where(c => c.OrderStatus == "Завершен").ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(Search))
                currentOrders = currentOrders.Where(p => p.OrderId.ToString().ToLower().Contains(Search.ToLower())).ToList();

            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По возрастанию":
                        currentOrders = currentOrders.OrderBy(o => o.OrderDate).ToList();
                        break;
                    case "По убыванию":
                        currentOrders = currentOrders.OrderByDescending(o => o.OrderDate).ToList();
                        break;
                }
            }

            Records = currentOrders.Count;
            Orders = currentOrders;
        }
        public DelegateCommand SignOutCommand => new(() =>
        {

            UserSetting.Default.UserName = string.Empty;
            UserSetting.Default.UserSurname = string.Empty;
            UserSetting.Default.UserPatronymic = string.Empty;
            UserSetting.Default.UserRole = string.Empty;
            _pageService.ChangePage(new SingInPage());
        });
        public DelegateCommand EditOrderCommand => new(() =>
        {
            Debug.WriteLine(SelectedOrder.OrderId.ToString());
        });
        public DelegateCommand HelpCommand => new(async () =>
        {
            //await _productService.UpdateAmmountOrder();
        });
    }
}
