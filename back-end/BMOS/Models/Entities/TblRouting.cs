using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMOS.Models.Entities;

public partial class TblRouting
{
    [NotMapped]
    public string[] listProductId { get; set; }
    [NotMapped]
    public List<SelectListItem> listProduct { get; set; }
    public string RoutingId { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<TblProductInRouting> TblProductInRoutings { get; set; } = new List<TblProductInRouting>();
}
