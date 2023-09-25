using System;
using System.Collections.Generic;

namespace LastguyShop.Data.Entities;

public partial class ProductFile
{
    public int ProductFileId { get; set; }

    public int ProductId { get; set; }

    public int FileId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public byte? IsDelete { get; set; }
}
