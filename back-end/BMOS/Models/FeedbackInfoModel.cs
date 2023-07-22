namespace BMOS.Models
{
	public class FeedbackInfoModel
	{
		public string FeedbackId { get; set; } = null!;
		public string? ProductId { get; set; }
		public string? Content { get; set; }
		public int? Star { get; set; }
		public string? fullName { get; set; }
		public int? UserId { get; set; }
		public DateTime? date { get; set; }
	}
}
