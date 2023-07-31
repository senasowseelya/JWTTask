using System;
using System.Collections.Generic;

namespace JWTDemo.DataContext;

public partial class UserRole
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public virtual RolesMaster Role { get; set; } = null!;

    public virtual UsersMaster User { get; set; } = null!;
}
