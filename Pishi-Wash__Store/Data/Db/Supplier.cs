using System;
using System.Collections.Generic;

namespace Pishi_Wash__Store.Data.Db;
public partial class Supplier
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Inn { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public int? QualityRating { get; set; }

    public string? SupplierType { get; set; }
}
