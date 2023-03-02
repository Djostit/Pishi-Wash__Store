using System;
using System.Collections.Generic;

namespace Pishi_Wash__Store.Data.Db;
public partial class Pmanufacturer
{
    public int PmanufacturerId { get; set; }

    public string ProductManufacturer { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
