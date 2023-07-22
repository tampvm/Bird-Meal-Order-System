using System;
using System.Collections.Generic;

namespace BMOS.Models.Entities;

public partial class TblVoucherCode
{
    public string VoucherId { get; set; } = null!;

    public string? VoucherCode { get; set; }

    public double? Value { get; set; }

    public int? Quantity { get; set; }

    public int? Used { get; set; }

    public bool? Status { get; set; }
}
