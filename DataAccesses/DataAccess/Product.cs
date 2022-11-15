using System;
using System.Collections.Generic;

namespace DataAccesses.DataAccess;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string ProductName { get; set; }

    public string Weight { get; set; }

    public decimal UnitPrice { get; set; }

    public int UnitInStock { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
