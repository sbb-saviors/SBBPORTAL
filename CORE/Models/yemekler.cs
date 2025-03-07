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

    public long? KategoriId { get; set; }

    public virtual yemek_kategorileri? Kategori { get; set; }

    public virtual ICollection<yemek_tarihleri_yemekler> yemek_tarihleri_yemeklers { get; set; } = new List<yemek_tarihleri_yemekler>();
}
