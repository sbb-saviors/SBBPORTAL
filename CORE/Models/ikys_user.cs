using System;
using System.Collections.Generic;

namespace CORE.Models;

public partial class ikys_user
{
    public long? Id { get; set; }

    public string? UserName { get; set; }

    public string? UserPassword { get; set; }

    public string? DisplayName { get; set; }

    public string? Role { get; set; }

    public bool? UserStatus { get; set; }

    public long? TcKimlikNumarasi { get; set; }

    public string? Guid { get; set; }

    public string? Fotograf { get; set; }

    public string? Base64Photo { get; set; }
}
