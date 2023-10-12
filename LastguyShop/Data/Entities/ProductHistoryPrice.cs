using System;
using System.Collections.Generic;

namespace LastguyShop.Data.Entities;

public partial class ProductHistoryPrice
{
    public int ProductId { get; set; }

    public int HistoryPriceId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public byte? IsDelete { get; set; }

    public int ProductHistoryPriceId { get; set; }
}
