﻿using BMOS.Models.Entities;

namespace BMOS.Models { 

	public class CartModel
	{
		public string _productId { get; set; }
		public string _productName { get; set; }
		public string _productImage { get; set; }
		public int? _quantity { get; set; }

		public double? _weight { get; set; }
		public double? _price { get; set; } = 0;
		public int? sold { get; set; }
		public int? productAvailable { get; set; }
		public int? getProductAvailable()
		{
			return this.productAvailable - this.sold;
		}
		public double? _getTotalPrice()
		{
			return this._price * this._quantity;
		}
	}
}
