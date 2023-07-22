namespace BMOS.Models
{
    public class RefundForm
    {
        public string refundId { get; set; }
        public string orderId { get; set; }

        public string content { get; set; }
        public bool status { get; set; }
        public int userId { get; set; }

        public DateTime date { get; set; }
    }
}