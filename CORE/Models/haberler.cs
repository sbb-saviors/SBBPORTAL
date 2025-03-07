using System;
using System.Collections.Generic;

namespace CORE.Models;

public partial class haberler
{
    public long Id { get; set; }

    public string? Baslik { get; set; }

    public string? Aciklama { get; set; }

    public string? Icerik { get; set; }

    public string? GaleriId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Kapak { get; set; }

    public bool? SilindiMi { get; set; }
}
