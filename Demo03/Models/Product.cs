using System;
using System.Collections.Generic;

namespace Demo03.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int UnitsInStock { get; set; }
}
