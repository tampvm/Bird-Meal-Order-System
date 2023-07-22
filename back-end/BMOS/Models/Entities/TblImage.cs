using System;
using System.Collections.Generic;

namespace BMOS.Models.Entities;

public partial class TblImage
{
    public string ImageId { get; set; } = null!;

    public string? Name { get; set; }

    public string? RelationId { get; set; }

    public string? Type { get; set; }

    public string? Url { get; set; }
}
