namespace Pishi_Wash__Store.Models
{
    public class DbProduct : Product
    {
        public string DisplayedImage => $"pack://application:,,,/Resources/Image/{ProductPhoto = (ProductPhoto == string.Empty ? "picture.png" : ProductPhoto)}";
        public float? DisplayedPrice => ProductDiscountAmount != 0 ? ProductCost - (ProductCost * ProductDiscountAmount / 100) : null;
        public int Count { get; set; }

    }
}
