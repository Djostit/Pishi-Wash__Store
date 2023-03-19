namespace Pishi_Wash__Store.Data.Models;

public partial class Point
{
    public int PointId { get; set; }

    public int Index { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public int House { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
