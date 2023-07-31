using System;
using System.Collections.Generic;

namespace JWTDemo.DataContext;

public partial class UsersMaster
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
