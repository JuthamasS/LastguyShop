using System;
using System.Collections.Generic;

namespace LastguyShop.Data.Entities;

public partial class FileUpload
{
    public string? FolderPath { get; set; }

    public string? FileName { get; set; }

    public string? FileNameContent { get; set; }

    public string? Mimetype { get; set; }

    public int? Size { get; set; }

    public DateTime? CreatedDate { get; set; }

    public byte? IsDelete { get; set; }

    public int FileId { get; set; }
}
