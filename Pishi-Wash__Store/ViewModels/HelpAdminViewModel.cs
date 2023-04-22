using System.Windows.Threading;

namespace Pishi_Wash__Store.ViewModels
{
    public class HelpAdminViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<string> Sorts { get; set; } = new() { "По возрастанию", "По убыванию" };
        public List<string> Filters { get; set; } = new() { "Все диапазоны", "Бумага офисная", "Для офиса", "Тетради школьные", "Школьные пренадлежности", "Школьные принадлежности" };
        public List<DbProduct> Products { get; set; }
        public List<Pmanufacturer> Pmanufacturers { get; set; }
        public List<Pcategory> Pcategories { get; set; }
        public DbProduct SelectedProduct { get; set; }
        public string FullName { get; set; } = UserSetting.Default.UserName == string.Empty ? "Гость" : $"{UserSetting.Default.UserSurname} {UserSetting.Default.UserName} {UserSetting.Default.UserPatronymic}";
        public HelpAdminViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
        }
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

        public TabItem SelectedItemTab
        {
            get { return GetValue<TabItem>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }
        private void SwitchItemTab()
        {
            if (SelectedItemTab == null)
                return;
            switch (SelectedItemTab.Header)
            {
                case "Продукты":
                    Filters = new() { "Все диапазоны", "Бумага офисная", "Для офиса", "Тетради школьные", "Школьные пренадлежности", "Школьные принадлежности" };
                    break;
                case "Поставщики":
                    Filters = new() { "Все диапазоны" };
                    break;
                case "Категории":
                    Filters = new() { "Все диапазоны" };
                    break;
            }
        }
        private async void UpdateProduct()
        {
            if (SelectedItemTab == null)
                return;
            SwitchItemTab();
            switch (SelectedItemTab.Header)
            {
                case "Продукты":
                    await Task.Run(async () =>
                    {
                        if (SelectedFilter != "Все диапазоны")
                            _productService.GetPcategories();
                        var currentProducts = await _productService.GetProducts();
                        MaxRecords = currentProducts.Count;

                        if (!string.IsNullOrEmpty(SelectedFilter))
                        {
                            switch (SelectedFilter)
                            {
                                case "Бумага офисная":
                                    currentProducts = currentProducts.Where(c => c.ProductCategoryNavigation.ProductCategory == "Бумага офисная").ToList();
                                    break;
                                case "Для офиса":
                                    currentProducts = currentProducts.Where(c => c.ProductCategoryNavigation.ProductCategory == "Для офиса").ToList();
                                    break;
                                case "Тетради школьные":
                                    currentProducts = currentProducts.Where(c => c.ProductCategoryNavigation.ProductCategory == "Тетради школьные").ToList();
                                    break;
                                case "Школьные пренадлежности":
                                    currentProducts = currentProducts.Where(c => c.ProductCategoryNavigation.ProductCategory == "Школьные пренадлежности").ToList();
                                    break;
                                case "Школьные принадлежности":
                                    currentProducts = currentProducts.Where(c => c.ProductCategoryNavigation.ProductCategory == "Школьные принадлежности").ToList();
                                    break;
                            }
                        }

                        if (!string.IsNullOrEmpty(Search))
                            currentProducts = currentProducts.Where(p => p.ProductNameNavigation.ProductName
                            .ToString()
                            .ToLower()
                            .Contains(Search.ToLower())
                            || p.ProductArticleNumber.ToLower().Contains(Search.ToLower())).ToList();

                        if (!string.IsNullOrEmpty(SelectedSort))
                        {
                            switch (SelectedSort)
                            {
                                case "По возрастанию":
                                    currentProducts = currentProducts.OrderBy(o => o.ProductNameNavigation.ProductName).ToList();
                                    break;
                                case "По убыванию":
                                    currentProducts = currentProducts.OrderByDescending(o => o.ProductNameNavigation.ProductName).ToList();
                                    break;
                            }
                        }

                        Records = currentProducts.Count;
                        Products = currentProducts;
                    });
                    break;

                case "Поставщики":
                    await Task.Run(() =>
                    {
                        var currnetPmanufacturers = _productService.GetPmanufacturers();
                        MaxRecords = currnetPmanufacturers.Count;

                        if (!string.IsNullOrEmpty(Search))
                            currnetPmanufacturers = currnetPmanufacturers.Where(p => p.ProductManufacturer
                            .ToString()
                            .ToLower()
                            .Contains(Search.ToLower())).ToList();

                        if (!string.IsNullOrEmpty(SelectedSort))
                        {
                            switch (SelectedSort)
                            {
                                case "По возрастанию":
                                    currnetPmanufacturers = currnetPmanufacturers.OrderBy(o => o.PmanufacturerId).ToList();
                                    break;
                                case "По убыванию":
                                    currnetPmanufacturers = currnetPmanufacturers.OrderByDescending(o => o.PmanufacturerId).ToList();
                                    break;
                            }
                        }

                        Records = currnetPmanufacturers.Count;
                        Pmanufacturers = currnetPmanufacturers;
                    });
                    break;

                case "Категории":
                    await Task.Run(() =>
                    {
                        var currentPcategories = _productService.GetPcategories();
                        MaxRecords = currentPcategories.Count;

                        if (!string.IsNullOrEmpty(Search))
                            currentPcategories = currentPcategories.Where(p => p.ProductCategory
                            .ToString()
                            .ToLower()
                            .Contains(Search.ToLower())).ToList();

                        if (!string.IsNullOrEmpty(SelectedSort))
                        {
                            switch (SelectedSort)
                            {
                                case "По возрастанию":
                                    currentPcategories = currentPcategories.OrderBy(o => o.PcategoryId).ToList();
                                    break;
                                case "По убыванию":
                                    currentPcategories = currentPcategories.OrderByDescending(o => o.PcategoryId).ToList();
                                    break;
                            }
                        }

                        Records = currentPcategories.Count;
                        Pcategories = currentPcategories;
                    });
                    break;
            }
        }
        public DelegateCommand EditProductCommand => new(() => Debug.WriteLine(" "));
        public DelegateCommand BrowseAdminCommand => new(() => _pageService.ChangePage(new BrowseAdminPage()));
        public DelegateCommand SignOutCommand => new(() =>
        {

            UserSetting.Default.UserName = string.Empty;
            UserSetting.Default.UserSurname = string.Empty;
            UserSetting.Default.UserPatronymic = string.Empty;
            UserSetting.Default.UserRole = string.Empty;
            _pageService.ChangePage(new SingInPage());
        });
    }
}
