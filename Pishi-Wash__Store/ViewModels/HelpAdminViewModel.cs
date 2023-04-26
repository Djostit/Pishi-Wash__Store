namespace Pishi_Wash__Store.ViewModels
{
    public class HelpAdminViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<string> Sorts { get; set; } = new() { "По возрастанию", "По убыванию" };
        public List<string> Filters { get; set; } = new() { "Все диапазоны", "Бумага офисная", "Для офиса", "Тетради школьные", "Школьные пренадлежности", "Школьные принадлежности" };
        public List<DbProduct> Products { get; set; }
        public ObservableCollection<Pmanufacturer> Pmanufacturers { get; set; }
        public ObservableCollection<Pcategory> Pcategories { get; set; }
        public ObservableCollection <Pprovider> Pproviders { get; set; }
        public ObservableCollection <Pname> Pnames { get; set; }
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

                case "Производители":
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
                        Pmanufacturers = new ObservableCollection<Pmanufacturer>(currnetPmanufacturers);
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
                        Pcategories = new ObservableCollection<Pcategory>(currentPcategories);
                    });
                    break;
                case "Поставщики":

                    break;
                case "Имена":

                    break;

            }
        }

        #region Product

        public DbProduct SelectedProduct { get; set; }

        // Редактирование
        public bool IsDialogEditProductOpen { get; set; } = false;
        public string EditProduct { get; set; }

        public DelegateCommand EditProductCommand => new(() =>
        {
            if (SelectedProduct == null)
                return;
            IsDialogEditProductOpen = true;
        });

        public DelegateCommand SaveCurrentProductCommand => new(async () =>
        {
            //if (SelectedManufacturers.ProductManufacturer != EditManufacturers
            //&& !Pmanufacturers.Any(p => p.ProductManufacturer == EditManufacturers))
            //{
            //    var item = Pmanufacturers.First(i => i.PmanufacturerId == SelectedManufacturers.PmanufacturerId);
            //    var index = Pmanufacturers.IndexOf(item);
            //    item.ProductManufacturer = EditManufacturers;

            //    Pmanufacturers.RemoveAt(index);
            //    Pmanufacturers.Insert(index, item);
            //    await _productService.SaveChangesAsync();
            //}
            IsDialogEditProductOpen = false;
        });

        // Добавление 
        public bool IsDialogAddProductOpen { get; set; } = false;
        public string ProductArticle { get; set; }
        public Pname ProductSelectedName { get; set; }
        public string ProductDescription { get; set; }
        public Pcategory ProductSelectedCategories { get; set; }
        public string ProductImage { get; set; }
        public Pmanufacturer ProductSelectedManufacturer { get; set; }
        public Pprovider ProductSelectedProvider { get; set; }
        public float ProductPrice { get; set; }
        public int ProductDiscount { get; set;}
        public int ProductCountInStock { get; set; }

        public DelegateCommand AddProductCommand => new(() =>
        {
            IsDialogAddProductOpen = true;
        });

        public DelegateCommand SaveAddProductCommand => new(async () =>
        {
            //if (!string.IsNullOrWhiteSpace(AddManufacturers)
            //&& !Pmanufacturers.Any(p => p.ProductManufacturer == AddManufacturers))
            //{
            //    Pmanufacturers.Insert(0, await _productService.AddManufacturersAsync(new Pmanufacturer
            //    {
            //        ProductManufacturer = AddManufacturers
            //    }));
            //}
            IsDialogAddProductOpen = false;
        }, bool() =>
        {
            return !string.IsNullOrWhiteSpace(ProductArticle)
            && ProductSelectedName != null
            && !string.IsNullOrWhiteSpace(ProductDescription)
            && ProductSelectedCategories != null
            && !string.IsNullOrWhiteSpace(ProductImage)
            && ProductSelectedManufacturer != null
            && ProductSelectedProvider != null
            && !float.IsNaN(ProductPrice)
            && ProductDiscount >= 0
            && ProductCountInStock >= 0;
        });


        #endregion

        #region Caterories

        public Pcategory SelectedCategories { get; set; }

        // Редактирование
        public bool IsDialogEditCategoriesOpen { get; set; } = false;
        public string EditCategories { get; set; }

        public DelegateCommand EditCategoriesCommand => new(() =>
        {
            if (SelectedCategories == null)
                return;
            EditCategories = SelectedCategories.ProductCategory;
            IsDialogEditCategoriesOpen = true;
        });

        public DelegateCommand SaveCurrentCategoriesCommand => new(async () =>
        {
            if (SelectedCategories.ProductCategory != EditCategories
            && !Pcategories.Any(p => p.ProductCategory == EditCategories))
            {
                var item = Pcategories.First(i => i.PcategoryId == SelectedCategories.PcategoryId);
                var index = Pcategories.IndexOf(item);
                item.ProductCategory = EditCategories;

                Pcategories.RemoveAt(index);
                Pcategories.Insert(index, item);
                await _productService.SaveChangesAsync();
            }
            IsDialogEditCategoriesOpen = false;
        });

        // Добавление 
        public bool IsDialogAddCategoriesOpen { get; set; } = false;
        public string AddCategories { get; set; } 
        public DelegateCommand AddCategoriesCommand => new(() =>
        {
            AddCategories = string.Empty;
            IsDialogAddCategoriesOpen = true;
        });

        public DelegateCommand SaveAddCategoriesCommand => new(async () =>
        {
            if (!string.IsNullOrWhiteSpace(AddCategories)
            && !Pcategories.Any(p => p.ProductCategory == AddCategories))
            {
                Pcategories.Insert(0, await _productService.AddCategoriesAsync(new Pcategory 
                { 
                    ProductCategory = AddCategories 
                }));
            }
            IsDialogAddCategoriesOpen = false;
        }, bool () =>
        {
            return !string.IsNullOrWhiteSpace(AddCategories);
        });

        #endregion

        #region Manufacturers

        public Pmanufacturer SelectedManufacturers { get; set; }

        // Редактирование
        public bool IsDialogEditManufacturersOpen { get; set; } = false;
        public string EditManufacturers { get; set; }

        public DelegateCommand EditManufacturersCommand => new(() =>
        {
            if (SelectedManufacturers == null)
                return;
            EditManufacturers = SelectedManufacturers.ProductManufacturer;
            IsDialogEditManufacturersOpen = true;
        });

        public DelegateCommand SaveCurrentManufacturersCommand => new(async () =>
        {
            if (SelectedManufacturers.ProductManufacturer != EditManufacturers
            && !Pmanufacturers.Any(p => p.ProductManufacturer == EditManufacturers))
            {
                var item = Pmanufacturers.First(i => i.PmanufacturerId == SelectedManufacturers.PmanufacturerId);
                var index = Pmanufacturers.IndexOf(item);
                item.ProductManufacturer = EditManufacturers;

                Pmanufacturers.RemoveAt(index);
                Pmanufacturers.Insert(index, item);
                await _productService.SaveChangesAsync();
            }
            IsDialogEditManufacturersOpen = false;
        });

        // Добавление 
        public bool IsDialogAddManufacturersOpen { get; set; } = false;
        public string AddManufacturers { get; set; }
        public DelegateCommand AddManufacturersCommand => new(() =>
        {
            AddManufacturers = string.Empty;
            IsDialogAddManufacturersOpen = true;
        });

        public DelegateCommand SaveAddManufacturersCommand => new(async () =>
        {
            if (!string.IsNullOrWhiteSpace(AddManufacturers)
            && !Pmanufacturers.Any(p => p.ProductManufacturer == AddManufacturers))
            {
                Pmanufacturers.Insert(0, await _productService.AddManufacturersAsync(new Pmanufacturer
                {
                    ProductManufacturer = AddManufacturers
                }));
            }
            IsDialogAddManufacturersOpen = false;
        }, bool () =>
        {
            return !string.IsNullOrWhiteSpace(AddManufacturers);
        });


        #endregion

        #region Provider

        #endregion

        #region Name

        #endregion



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
