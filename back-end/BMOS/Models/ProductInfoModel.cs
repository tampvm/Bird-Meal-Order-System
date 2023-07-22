using BMOS.Models.Entities;
using System;
using System.Collections.Generic;
namespace BMOS.Models
{
	public class ProductInfoModel : TblProduct
	{
		//public string ProductId { get; set; } = null!;

		//public string? Name { get; set; }

		//public double? Price { get; set; }

		//public string? Description { get; set; }

		//public double? Weight { get; set; }
		//public bool? IsAvailable { get; set; }

		//public int? SoldQuantity { get; set; }
		//public bool? Status { get; set; }	
		//public bool? IsLoved { get; set; }
		public string? UrlImage { get; set; }


		public List<RelatedProductModel>? relatedProductModels { get; set; }
	}
}
