namespace Pishi_Wash__Store.Data.test;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly OrderDeliveryDate { get; set; }

    public int OrderPickupPoint { get; set; }

    public string OrderFullName { get; set; } = null!;

    public int OrderCode { get; set; }

    public string OrderStatus { get; set; } = null!;

    public virtual Point OrderPickupPointNavigation { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; } = new List<Orderproduct>();
}
