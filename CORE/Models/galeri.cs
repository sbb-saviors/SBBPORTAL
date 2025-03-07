using System;
using System.Collections.Generic;

namespace CORE.Models;

public partial class galeri
{
    public long Id { get; set; }

    public string? GaleriId { get; set; }

    public string? Fotograf { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? SilindiMi { get; set; }
}
