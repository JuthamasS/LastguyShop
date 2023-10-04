using System;
using System.Collections.Generic;

namespace LastguyShop.Data.Entities;

public partial class HistoryPrice
{
    public int? Price { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte? IsDelete { get; set; }

    public int HistoryPriceId { get; set; }
}
