namespace Pishi_Wash__Store.Models
{
    public class Product
    {
        public string Image { get; set; }
        public string DisplayedImage
        {
            get { return Path.GetFullPath($@"Resources\Image\{Image}"); }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public float Price { get; set; }
        public int Discount { get; set; }

    }
}
