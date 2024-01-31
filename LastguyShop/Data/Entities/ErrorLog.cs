using System;
using System.Collections.Generic;

namespace LastguyShop.Data.Entities;

public partial class ErrorLog
{
    public int LogId { get; set; }

    public int? ReceiptId { get; set; }

    public DateTime? ErrorDate { get; set; }

    public int? ProductId { get; set; }
    public string? Note { get; set; }

}
