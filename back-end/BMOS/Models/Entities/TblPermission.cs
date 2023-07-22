using System;
using System.Collections.Generic;

namespace BMOS.Models.Entities;

public partial class TblPermission
{
    public int PermissionId { get; set; }

    public int? UserRoleId { get; set; }

    public string? PermissionName { get; set; }
}
