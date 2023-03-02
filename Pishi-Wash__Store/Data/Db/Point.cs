using System;
using System.Collections.Generic;

namespace Pishi_Wash__Store.Data.Db;
public partial class Point
{
    public uint Index { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public int House { get; set; }
}
