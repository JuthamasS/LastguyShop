using System;
using System.Collections.Generic;

namespace LastguyShop.Entities;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? OfficeHours { get; set; }

    public string? Workday { get; set; }

    public string? LineId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
