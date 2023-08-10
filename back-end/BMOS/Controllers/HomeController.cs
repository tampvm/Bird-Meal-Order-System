using BMOS.Models.Entities;
using BMOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BMOS.Helpers;
using Firebase.Auth;

namespace BMOS.Controllers
{
	public class HomeController : Controller
	{
		private readonly BmosContext _context;

		public HomeController(BmosContext context)
		{
			_context = context;
		}

		// GET: TblProducts
		[HttpGet]
		public async Task<IActionResult> Index(string searchString)
		{
			var confirmOrderStatus = HttpContext?.Session.Get<bool>("confirmOrderStatus");
			HttpContext?.Session.Remove("Routing");

			var routingList = from routing in _context.TblRoutings
							  from image in _context.TblImages
							  where routing.RoutingId == image.RelationId && routing.Status != false
							  select new RoutingHomePageModel
							  {
								  routingId = routing.RoutingId,
								  routingName = routing.Name,
								  routingPrice = routing.Price,
								  routingImage = image.Url,
							  };
			ViewData["Routing"] = routingList.ToList();
			var blogList = from blog in _context.TblBlogs
						   from image in _context.TblImages
						   where blog.BlogId == image.RelationId && blog.Status != false
						   select new BlogInfoModel
						   {
							   blogId = blog.BlogId,
							   blogName = blog.Name,
							   blogDescription = blog.Description,
							   blogImage = image.Url,
							   Date = blog.Date,
						   };
			ViewData["Blog"] = blogList.ToList();
			ViewBag.isConfirmOrder = confirmOrderStatus;

			if (!String.IsNullOrEmpty(searchString))
			{
				return RedirectToAction("ListProduct", "Products", new { searchString });
			}

			var listProdct = from product in _context.TblProducts
							 from image in _context.TblImages
							 where product.ProductId == image.RelationId && product.Status != false
							 select new
							 {
								 productId = product.ProductId,
								 productName = product.Name,
								 productPrice = product.Price,
								 productImage = image.Url
							 };


			//notify
			var user = HttpContext?.Session.Get<TblUser>("user");
			if (user != null)
			{
				var orderList = _context.TblOrders.Where(p => p.UserId == user.UserId).ToList();
				var resultOrderList = orderList.OrderByDescending(x => x.Date.ToString()).ToList();
				double? currentPoint = 0;
				if (user.Point != null)
				{
					currentPoint += user.Point;
				}
				else
				{
					user.Point = 0;
				}
				foreach (var order in resultOrderList)
				{
					var twoDayAgo = DateTime.Now.AddDays(-5);
					if (order.Date < twoDayAgo && order.Point > 0)
					{
						if ((bool)order.IsConfirm)
						{
							//check order id is exsit in notify table.
							var checkOrderId = _context.TblNotifies.Where(x => x.Message.Contains(order.OrderId)).FirstOrDefault();
							if (checkOrderId == null)
							{
								currentPoint += order.Point;
								user.Point = currentPoint;
								TblNotify notify = new TblNotify
								{
									NotifyId = Guid.NewGuid().ToString(),
									UserId = user.UserId,
									Message = "Bạn được tích " + order.Point + " điểm ở đơn hàng " + order.OrderId,
									Type = "Point",
									Date = DateTime.Now,
								};
								_context.Update(user);
								_context.Add(notify);
								_context.SaveChanges();
							}
							else
							{
								break;
							}
						}
					}
				}

				var tblNotify = _context.TblNotifies.Where(x => x.UserId.Equals(user.UserId)).ToList();
				if (tblNotify.Count() > 0)
				{
					ViewData["Notify"] = tblNotify;
				}

			}


			HttpContext?.Session.Remove("confirmOrderStatus");
			return listProdct != null ? View(await listProdct.ToListAsync()) : Problem("Entity set 'BmosContext.TblProducts' is null");
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}

}