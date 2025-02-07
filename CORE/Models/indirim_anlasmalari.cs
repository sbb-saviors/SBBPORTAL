using System;
using System.Collections.Generic;

namespace CORE.Models;

public partial class indirim_anlasmalari
{
    public long Id { get; set; }

    public string? Kategori { get; set; }

    public string? Baslik { get; set; }

    public string? Aciklama { get; set; }

    public DateTime? OlusturmaTarihi { get; set; }

    public bool? SilindiMi { get; set; }
}
