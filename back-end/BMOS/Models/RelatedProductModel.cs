using BMOS.Models.Entities;
using System.Collections.Generic;
namespace BMOS.Models
{
    public class RelatedProductModel
    {
        public string _id { get; set; }
        public string _prodName { get; set; }
        public double? _prodPrice { get; set; }
        public string _image { get; set; }

		internal object ToList()
		{
			throw new NotImplementedException();
		}
	}
}
