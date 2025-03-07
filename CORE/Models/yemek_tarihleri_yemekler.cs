using System;
using System.Collections.Generic;

namespace CORE.Models;

public partial class yemek_tarihleri_yemekler
{
    public long Id { get; set; }

    public long? TarihId { get; set; }

    public long? YemekId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? SilindiMi { get; set; }

    public virtual yemek_tarihleri? Tarih { get; set; }

    public virtual yemekler? Yemek { get; set; }
}
