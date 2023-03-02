using System.Collections.ObjectModel;

namespace Pishi_Wash__Store.ViewModels
{
    public class CartViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public ObservableCollection<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }
        public float OrderAmmount { get; set; } = 0;
        public int DiscountAmmount { get; set; } = 0;
        public string FullName { get; set; } = UserSetting.Default.UserName == string.Empty ? "Гость" : $"{UserSetting.Default.UserSurname} {UserSetting.Default.UserName} {UserSetting.Default.UserPatronymic}";

        public CartViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            Task.Run(async () => { Products = new ObservableCollection<Product>(await _productService.GetCart()); ValueCheck(); });
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
        private void ValueCheck()
        {
            OrderAmmount = 0;
            DiscountAmmount = 0;
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
                    DiscountAmmount += item.Discount;
                }
            }
                
        }
    }
}
