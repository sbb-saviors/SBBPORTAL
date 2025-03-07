using System;
using System.Collections.Generic;

namespace CORE.Models;

public partial class yemek_kategorileri
{
    public long Id { get; set; }

    public string? KategoriAdi { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? SilindiMi { get; set; }

    public virtual ICollection<yemekler> yemeklers { get; set; } = new List<yemekler>();
}
