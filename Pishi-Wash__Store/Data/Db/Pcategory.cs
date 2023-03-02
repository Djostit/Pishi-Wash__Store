using System;
using System.Collections.Generic;

namespace Pishi_Wash__Store.Data.Db;

public partial class Pcategory
{
    public int PcategoryId { get; set; }

    public string ProductCategory { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
