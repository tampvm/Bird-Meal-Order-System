using Microsoft.Identity.Client;
using Org.BouncyCastle.Ocsp;
using System.Text;

namespace BMOS.Models
{
    public class BlogInfoModel
    {
        public string? blogId { get; set; }
        public string? blogName { get; set; }
        public string? blogDescription { get; set; }
        public string? blogImage { get; set; }
        public DateTime? Date { get; set; } 

    }
}
