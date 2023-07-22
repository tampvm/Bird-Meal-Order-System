using System;
using System.Collections.Generic;

namespace BMOS.Models.Entities;

public partial class TblOrder
{
    public string OrderId { get; set; } = null!;

    public int? UserId { get; set; }

    public double? TotalPrice { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsConfirm { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Note { get; set; }

    public string? PaymentType { get; set; }

    public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; } = new List<TblOrderDetail>();

    public virtual ICollection<TblRefund> TblRefunds { get; set; } = new List<TblRefund>();
}
