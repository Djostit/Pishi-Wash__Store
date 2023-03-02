using System;
using System.Collections.Generic;

namespace Pishi_Wash__Store.Data.Db;
public partial class Producttype
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public double DefectedPercent { get; set; }
}
