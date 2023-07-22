using System;
using System.Collections.Generic;

namespace BMOS.Models.Entities;

public partial class TblRole
{
    public int UserRoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual TblPermission UserRole { get; set; } = null!;

    public virtual TblUser UserRoleNavigation { get; set; } = null!;
}
