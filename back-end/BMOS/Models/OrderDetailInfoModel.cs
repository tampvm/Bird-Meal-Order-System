using BMOS.Models.Entities;

namespace BMOS.Models
{
	public class OrderDetailInfoModel : TblOrderDetail
	{
		public string? productName { get; set; }	
		public string? image { get; set;}
	}
}
