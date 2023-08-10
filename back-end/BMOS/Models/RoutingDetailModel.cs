using BMOS.Models.Entities;
using System.Transactions;

namespace BMOS.Models
{
    public class RoutingDetailModel 
    {
        public string? productId { get; set; }
        public string? productName { get; set; } 
        public string? description { get; set; }
        public double? productPrice { get; set; }
        public string? image { get; set; }

        public int? sold { get; set; }   
        public int? quantity { get; set; }
        
        public int productQuantity { get; set; } = 1;

        public double? getTotalProductPrice()
        {
            return this.productQuantity * this.productPrice;
        }

        public int? getAvailableProduct()
        {   
            return this.quantity - this.sold;
        } 
	}

 
}
