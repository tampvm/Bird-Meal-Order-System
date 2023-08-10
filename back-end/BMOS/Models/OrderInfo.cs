using BMOS.Models.Entities;

namespace BMOS.Models
{
    public class OrderInfo
    {
        public string? orderID {  get; set; }

        public string? UserName { get; set; }

        public int Quantity { get; set; }

        public DateTime? date { get; set; }

        public double? total { get; set; }
        public bool? IsConfirm { get; set; }

        public List<TblProduct>? proname { get; set; }
        public int? Status { get; set; }
        public string Note { get; set; }
    }
}
