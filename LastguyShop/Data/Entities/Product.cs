using System;
using System.Collections.Generic;

namespace LastguyShop.Data.Entities;

public partial class Product
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Note { get; set; }

    public int? Price { get; set; }

    public int? TotalAmount { get; set; }

    public string? Unit { get; set; }

    public int? SafetyStockNumber { get; set; }

    public int? HistoryId { get; set; }

    public int? SupplierId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte? IsDelete { get; set; }

    public int ProductId { get; set; }

    public byte? IsFavorite { get; set; }
}
