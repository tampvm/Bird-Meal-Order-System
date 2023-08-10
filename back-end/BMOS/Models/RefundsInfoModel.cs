namespace BMOS.Models
{
    public class RefundsInfoModel
    {
        public string? RefundId { get; set; }   
        public string? OrderId { get; set;}
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Description { get; set;}
        public DateTime?  Date { get; set; }
        public bool? IsConfirm { get; set; }
        public string urlImage { get; set; }
    }
}
