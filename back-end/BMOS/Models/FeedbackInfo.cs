namespace BMOS.Models
{
    public class FeedbackInfo
    {
        public string FeedbackId { get; set; } = null!;
        public string? Name { get; set;}
        public string? Content { get; set;}
        public int? Star { get; set;}

        public string userName { get; set; }
        public DateTime? date { get; set;}
        public string urlImage { get; set; }
    }
}
