using System;
using System.Collections.Generic;

namespace CORE.Models;

public partial class ikys_refresh_token
{
    public long? RefreshTokenId { get; set; }

    public string? Token { get; set; }

    public DateTime? Expires { get; set; }

    public DateTime? Created { get; set; }

    public string? CreatedByIp { get; set; }

    public DateTime? Revoked { get; set; }

    public string? RevokedByIp { get; set; }

    public string? ReplacedByToken { get; set; }

    public string? ReasonRevoked { get; set; }

    public bool? IsExpired { get; set; }

    public bool? IsRevoked { get; set; }

    public bool? IsActive { get; set; }

    public long? UserId { get; set; }
}
