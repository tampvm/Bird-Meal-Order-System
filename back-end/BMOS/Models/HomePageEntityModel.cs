using BMOS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BMOS.Models
{
    public class HomePageEntityModel
    {
        public string productId { get; set; }
        public string productName { get; set; } 
        public double? productPrice { get; set; }
        public string productImage { get; set; }

        public virtual DbSet<TblBlog> TblBlogs { get; set; }

    }
}
