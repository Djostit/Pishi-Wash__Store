namespace Pishi_Wash__Store.Models
{
    public class DbOrder : Order
    {
        public string BackgroundColor => Orderproducts.Any(o => o.ProductArticleNumberNavigation.ProductQuantityInStock > 3) ? "#ff8c00" : "#20b2aa";
    }
}
