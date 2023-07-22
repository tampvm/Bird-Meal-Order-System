using Microsoft.AspNetCore.Mvc.Rendering;

namespace BMOS.Models
{
    public class ProductInRoutingModel
    {
        public string productId { get; set; }
        public List<SelectListItem> productList { get; set; }
        public string[] listProductId { get; set; }    
    }
}
