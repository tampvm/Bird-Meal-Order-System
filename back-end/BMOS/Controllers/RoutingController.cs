using BMOS.Helpers;
using BMOS.Models;
using BMOS.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace BMOS.Controllers
{
	public class RoutingController : Controller
	{
		private BmosContext _context;
		public RoutingController(BmosContext context)
		{
			_context = context;
		}
		// GET: RoutingController
		public ActionResult Index()
		{
			return View();
		}

		public List<RoutingDetailModel> routingDetails
		{
			get
			{
				var data = HttpContext?.Session.Get<List<RoutingDetailModel>>("Routing");
				return data;
			}
		}

		public double? getTotalPrice()
		{
			double? result = 0;
			var myCart = HttpContext.Session.Get<List<RoutingDetailModel>>("Routing");
			foreach (var item in myCart.ToList())
			{
				result += item.getTotalProductPrice();
			}
			return result;
		}

		// GET: RoutingController/Details/5
		public IActionResult RoutingDetail(string id)
		{

			var routingList = from routing in _context.TblRoutings
							  from image in _context.TblImages
							  where image.RelationId == id && routing.Status != false
							  select new RoutingHomePageModel
							  {
								  routingId = routing.RoutingId,
								  routingName = routing.Name,
								  routingPrice = routing.Price,
								  routingImage = image.Url,
							  };
			ViewData["Routing"] = routingList.FirstOrDefault(x => x.routingId == id);
			var routingSession = routingDetails;
			if (routingSession == null)
			{
				routingSession = new List<RoutingDetailModel>();
				var productList = from prod in _context.TblProductInRoutings
								  from _prod in _context.TblProducts
								  from _img in _context.TblImages
								  where prod.RoutingId == id && _prod.ProductId == prod.ProductId && _prod.ProductId == _img.RelationId
								  select new RoutingDetailModel()
								  {
									  productId = _prod.ProductId,
									  productName = _prod.Name,
									  productPrice = _prod.Price,
									  productQuantity = 1,
									  description = _prod.Description,
									  image = _img.Url
								  };

				foreach (var product in productList.ToList())
				{
					routingSession.Add(product);
				}
				HttpContext.Session.Set("Routing", routingSession);
			}
			var totalPrice = getTotalPrice();
			ViewData["totalPrice"] = totalPrice;
			return View(routingSession);
		}

		public IActionResult EditRoutingDetail(string id, string status, int productQuantity = 1)
		{
			var totalPrice = getTotalPrice();
			var routingDetailsList = routingDetails;
			foreach (var item in routingDetailsList.ToList())
			{
				if (item.productId.Equals(id))
				{
					if (status.Equals("increase"))
					{
						item.productQuantity += productQuantity;
					}
					else if (status.Equals("decrease"))
					{
						item.productQuantity -= productQuantity;
						if (item.productQuantity == 0)
						{
							routingDetailsList.Remove(item);
						}
					}
				}
			}
			ViewData["totalPrice"] = totalPrice;
			HttpContext?.Session.Set("Routing", routingDetailsList);
			return PartialView(routingDetailsList);
		}
	}
}
