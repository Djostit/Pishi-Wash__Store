namespace Pishi_Wash__Store.ViewModels
{
    public class CartViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly DocumentService _documentService;
        private readonly static Random rnd = new();
        public ObservableCollection<DbProduct> Products { get; set; }
        public List<Point> Points { get; set; }
        public DbProduct SelectedProduct { get; set; }
        public Point SelectedPoint { get; set; }
        public float OrderAmmount { get; set; } = 0;
        public float DiscountAmmount { get; set; } = 0;
        private float _orderAmmount;
        public string FullName { get; set; } = UserSetting.Default.UserName == string.Empty ? "Гость" : $"{UserSetting.Default.UserSurname} {UserSetting.Default.UserName} {UserSetting.Default.UserPatronymic}";

        public CartViewModel(PageService pageService, ProductService productService, DocumentService documentService)
        {
            _pageService = pageService;
            _productService = productService;
            _documentService = documentService;
            Task.Run(async () => 
            { 
                Products = new ObservableCollection<DbProduct>(await _productService.GetCart()); 
                ValueCheck(); 
                Points = await _productService.GetPoints(); 
            });
        }

        public DelegateCommand ReturnBackCommand => new(() =>
        {
            _pageService.ChangePage(new BrowseProductPage());
        });

        public DelegateCommand SignOutCommand => new(() =>
        {
            UserSetting.Default.UserName = string.Empty;
            UserSetting.Default.UserSurname = string.Empty;
            UserSetting.Default.UserPatronymic = string.Empty;
            UserSetting.Default.UserRole = string.Empty;
            Global.CurrentCart.Clear();
            _pageService.ChangePage(new SingInPage());
        });

        public DelegateCommand RemoveCommand => new(() => 
        {
            if(SelectedProduct == null)
                return;
            var item = Products.First(i => i.Article.Equals(SelectedProduct.Article));
            var index = Products.IndexOf(item);
            item.Count--;

            var test = Global.CurrentCart.First(x => x.ArticleName.Equals(SelectedProduct.Article));
            var test2 = Global.CurrentCart.IndexOf(test);

            if (item.Count <= 0) 
            {
                Products.Remove(SelectedProduct);
                Global.CurrentCart.Remove(test);
            }
            else 
            {
                Products.RemoveAt(index);
                Products.Insert(index, item);
                Global.CurrentCart[test2].Count--;
            }
            ValueCheck();
        });
        
        public AsyncCommand CreateOrderCommand => new(async() =>
        {
            int code = rnd.Next(100, 999);
            await _documentService.GetCheck(OrderAmmount, DiscountAmmount, SelectedPoint, code, await _productService.AddOrder(new Order
            {
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderDeliveryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(Products.FirstOrDefault(a => a.Quantity < 3) != null ? 3 : 6)),
                OrderPickupPoint = SelectedPoint.PointId,
                OrderFullName = FullName == "Гость" ? string.Empty : FullName,
                OrderAmmount= OrderAmmount,
                OrderDiscountAmmount = DiscountAmmount,
                OrderCode = code,
                OrderStatus = "Новый"
            }), await _productService.GetListFullInformation());

            Products.Clear();
            Global.CurrentCart?.Clear();
            ValueCheck();
        }, bool() => { return SelectedPoint != null && Products.Count != 0; });
        private void ValueCheck()
        {
            OrderAmmount = 0;
            DiscountAmmount = 0;
            _orderAmmount = 0;
            if (Products.Count <= 0)
            {
                OrderAmmount = 0;
                DiscountAmmount = 0;
            }
            else
            {
                foreach (var item in Products)
                {
                    OrderAmmount += (item.Count * item.Price) - ((item.Count * item.Price) * item.Discount / 100);
                    _orderAmmount += item.Count * item.Price;
                }
                OrderAmmount = (float)Math.Round(OrderAmmount, 2);
                DiscountAmmount = (float)Math.Round((_orderAmmount - OrderAmmount), 2);
            }
        }
    }
}
