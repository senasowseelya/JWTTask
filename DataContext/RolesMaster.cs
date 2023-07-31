using System;
using System.Collections.Generic;

namespace JWTDemo.DataContext;

public partial class RolesMaster
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }
}
