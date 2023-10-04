using System;
using System.Collections.Generic;

namespace LastguyShop.Data.Entities;

public partial class Supplier
{
    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ContactName { get; set; }

    public string? OfficeHours { get; set; }

    public string? Workday { get; set; }

    public string? LineId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte? IsDelete { get; set; }

    public int SupplierId { get; set; }
}
