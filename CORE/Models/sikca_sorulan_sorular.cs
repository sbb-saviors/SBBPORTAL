﻿using System;
using System.Collections.Generic;

namespace CORE.Models;

public partial class sikca_sorulan_sorular
{
    public long Id { get; set; }

    public string? SoruBaslik { get; set; }

    public string? SoruCevap { get; set; }

    public DateTime? OlusturmaTarihi { get; set; }

    public bool? SilindiMi { get; set; }
}
