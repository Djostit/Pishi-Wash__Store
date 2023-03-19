namespace Pishi_Wash__Store.Data.Models;

public partial class Pprovider
{
    public int PproviderId { get; set; }

    public string ProductProvider { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
