﻿using System;
using System.Collections.Generic;

namespace BMOS.Models.Entities;

public partial class TblRefund
{
    public string RefundId { get; set; } = null!;

    public int? UserId { get; set; }

    public string? OrderId { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsConfirm { get; set; }

    public virtual TblOrder? Order { get; set; }

    public virtual TblUser? User { get; set; }
}
