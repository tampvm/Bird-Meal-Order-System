using BMOS.Models;
using BMOS.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BMOS.Controllers
{
	public class DashboardController : Controller
	{
		private readonly BmosContext _context;
		public DashboardController(BmosContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			DateTime startdate = DateTime.Today.AddYears(-1);
			DateTime enddate = DateTime.Today;
			var listorder = _context.TblOrders.Where(x => x.Date >= startdate && x.Date <= enddate).ToList();
			var listrefund = _context.TblRefunds.Where(x => x.IsConfirm == true).ToList();
			var listdetail = _context.TblOrderDetails.ToList();


			var totalprice = listorder.Where(x => x.IsConfirm == true).Sum(t => t.TotalPrice);
			ViewBag.TotalPrice = totalprice;
			var totalorder = listorder.Count(x => x.IsConfirm == true);
			ViewBag.Totalorder = totalorder;
			var totalrefund = listrefund.Count(x => x.IsConfirm == true);
			ViewBag.Totalrefund = totalrefund;
			var chartdb = listorder.Where(x => x.IsConfirm == true).GroupBy(a => a.Date.Value.Month).Select(k => new LineChart()
			{
				day = k.First().Date,
				price = k.Sum(l => l.TotalPrice)
			}).ToList();

			var toppro = listdetail.GroupBy(x => x.ProductId).Select(a => new Top3pro()
			{
				id = a.First().ProductId,
				quantity = a.Sum(l=>l.Quantity),
				price = a.Sum(l => l.Price * l.Quantity)
			}).ToList();

			var top3pro = from top in toppro
						  join top3 in _context.TblProducts on top.id equals top3.ProductId
						  select new top3final()
						  {
							  id = top.id,
							  name = top3.Name,
							  price = top.price,
							  quantity = top.quantity,							 
						  };

			top3pro = top3pro.OrderByDescending(x=>x.price).Take(3).ToList();

			DateTime[] last1year = Enumerable.Range(1, 12).Select(i => startdate.AddMonths(i)).ToArray();



			ViewBag.chartt = from _day in last1year
							 join money in chartdb on _day.Month equals money.getMonth() into daypricejoined
							 from price in daypricejoined.DefaultIfEmpty()
							 select new
							 {
								 day = _day,
								 price = price == null ? 0 : price.price
							 };

			return View(top3pro);
		}

		public class LineChart
		{
			public DateTime? day;
			public double? price;

			public int getMonth()
			{
				return day.Value.Month;
			}
		}
		public class Top3pro
		{
			public string? id;
			public int? quantity;
			public double? price;
		}
	}
}
