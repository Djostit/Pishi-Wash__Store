using static System.Net.Mime.MediaTypeNames;

namespace Pishi_Wash__Store.Models.DbContext
{
    public class DbProduct
    {
        [Key]
        public string ProductArticleNumber { get; set; }
        public int ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductCategory { get; set; }
        public string ProductPhoto { get; set; }
        public int ProductManufacturer { get; set; }
        public int ProductProvider { get; set; }
        public float ProductCost { get; set; }
        public int ProductDiscountAmount { get; set; }
        public int ProductQuantityInStock { get; set; }
        public string ProductStatus { get; set; }
    }
}
