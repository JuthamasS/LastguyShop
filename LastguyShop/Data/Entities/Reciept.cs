using System;
using System.Collections.Generic;

namespace LastguyShop.Data.Entities;

public partial class Receipt
{
    public int? ReceiptId { get; set; }

    public int? ProductId { get; set; }

    public DateTime? ReceiptDate { get; set; }

    public int? Amount { get; set; }

    public int? Cash { get; set; }

    public int? CutUnit { get; set; }

    public int? IsQRCodePayment { get; set; }


}
