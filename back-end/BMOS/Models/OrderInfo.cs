using BMOS.Models.Entities;

namespace BMOS.Models
{
    public class OrderInfo
    {
        public string? orderID {  get; set; }

        public string? UserName { get; set; }

        public int Quantity { get; set; }

        public string? date { get; set; }

        public double? total { get; set; }
        public bool? IsConfirm { get; set; }

        public List<TblProduct>? proname { get; set; }

    }
}
