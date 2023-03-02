using System;
using System.Collections.Generic;

namespace Pishi_Wash__Store.Data.Db;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderStatus { get; set; } = null!;

    public DateOnly OrderDeliveryDate { get; set; }

    public string OrderPickupPoint { get; set; } = null!;
}
