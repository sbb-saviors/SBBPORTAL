using System;
using System.Collections.Generic;

namespace CORE.Models;

public partial class yemekler
{
    public long Id { get; set; }

    public string YemekAdi { get; set; } = null!;

    public string? Aciklama { get; set; }

    public DateTime? OlusturmaTarihi { get; set; }

    public bool? SilindiMi { get; set; }

    public string? Fotograf { get; set; }
}
