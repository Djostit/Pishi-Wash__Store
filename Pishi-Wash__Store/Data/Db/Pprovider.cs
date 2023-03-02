using System;
using System.Collections.Generic;

namespace Pishi_Wash__Store.Data.Db;
public partial class Pprovider
{
    public int PproviderId { get; set; }

    public string ProductProvider { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
