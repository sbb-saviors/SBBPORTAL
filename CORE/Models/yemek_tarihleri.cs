using System;
using System.Collections.Generic;

namespace CORE.Models;

public partial class yemek_tarihleri
{
    public long Id { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? Tarih { get; set; }

    public bool? SilindiMi { get; set; }

    public virtual ICollection<yemek_tarihleri_yemekler> yemek_tarihleri_yemeklers { get; set; } = new List<yemek_tarihleri_yemekler>();
}
