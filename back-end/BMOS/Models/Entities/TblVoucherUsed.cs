using System;
using System.Collections.Generic;

namespace BMOS.Models.Entities;

public partial class TblVoucherUsed
{
    public int Id { get; set; }

    public string? VoucherId { get; set; }

    public int? UserId { get; set; }
}
