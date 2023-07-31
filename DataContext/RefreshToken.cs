using System;
using System.Collections.Generic;

namespace JWTDemo.DataContext;

public partial class RefreshToken
{
    public int RefreshTokenId { get; set; }

    public string RefreshToken1 { get; set; } = null!;

    public string Jwtid { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool Used { get; set; }

    public int UserId { get; set; }

    public virtual UsersMaster User { get; set; } = null!;
}
