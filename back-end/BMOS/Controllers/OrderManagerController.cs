using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BMOS.Models.Entities;
using BMOS.Models;
using X.PagedList;
using BMOS.Helpers;
using Syncfusion.EJ2.Linq;

namespace BMOS.Controllers
{
    public class OrderManagerController : Controller
    {
        private readonly BmosContext _context;

        public OrderManagerController(BmosContext context)
        {
            _context = context;
        }

        // GET: FeedbackManager
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var user = HttpContext.Session.Get<TblUser>("userManager");
            if (user != null)
            {
                if (user.UserRoleId == 1 || user.UserRoleId == 4)
                {
                    return View("ErrorPage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["SearchParameter"] = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var orderList = _context.TblOrders.ToList();
            foreach (var _order in orderList)
            {
                var fiveDay = DateTime.Now.AddDays(-5);
                if (_order.Date <= fiveDay && _order.Status == 2)
                {
                    _order.Status = 8;
                    _context.Update(_order);
                }
            }
            _context.SaveChanges();

            var order = from f in _context.TblOrders
                        join u in _context.TblUsers on f.UserId equals u.UserId
                        select new OrderInfo()
                        {
                            orderID = f.OrderId,
                            UserName = u.Firstname + u.Lastname,
                            date = f.Date,
                            total = f.TotalPrice,
                            IsConfirm = f.IsConfirm,
                            Status = f.Status,
                        };



            if (!String.IsNullOrEmpty(searchString))
            {
                order = order.Where(s => s.UserName.Contains(searchString));
                int count = order.Count();
                if (count == 0)
                {
                    ViewBag.Message = "No matches found";
                }
                else
                {
                    ViewBag.Message = "Có " + count + " kết quả tìm kiếm với từ khóa: " + searchString;
                }
            }

            switch (sortOrder)
            {
                case "name":
                    order = order.OrderBy(s => s.orderID);
                    break;
                case "name_desc":
                    order = order.OrderByDescending(s => s.orderID);
                    break;
                case "price":
                    order = order.OrderBy(s => s.total);
                    break;
                case "price_desc":
                    order = order.OrderByDescending(s => s.total);
                    break;
                default:
                    order = order.OrderBy(s => s.orderID);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(order.OrderByDescending(x => x.date).ToPagedList(pageNumber, pageSize));
        }


        // GET: FeedbackManager/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var user = HttpContext.Session.Get<TblUser>("userManager");
            if (user != null)
            {
                if (user.UserRoleId == 1)
                {
                    return View("ErrorPage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null || _context.TblFeedbacks == null)
            {
                return NotFound();
            }
            var order = from f in _context.TblOrders
                        join u in _context.TblUsers on f.UserId equals u.UserId
                        select new OrderInfo()
                        {
                            orderID = f.OrderId,
                            UserName = u.Firstname + u.Lastname,
                            Quantity = (int)_context.TblOrderDetails.Where(x => x.OrderId == f.OrderId).Sum(x => x.Quantity),
                            date = f.Date,
                            total = f.TotalPrice,
                            IsConfirm = f.IsConfirm
                        };
            var orderdetails = from d in _context.TblOrderDetails
                               from p in _context.TblProducts
                               from image in _context.TblImages
                               where (d.OrderId == id && p.ProductId.Equals(d.ProductId)) && (d.ProductId.Equals(image.RelationId))
                               select new OrderdetailsInfo()
                               {
                                   orderId = d.OrderId,
                                   namepro = p.Name,
                                   quantity = (int)d.Quantity,
                                   price = (double)(d.Price * d.Quantity),
                                   urlImage = image.Url,
                               };
            var orderdetail = orderdetails.Where(p => p.orderId == id).ToList();
            ViewData["OrderDetails"] = orderdetail.ToList();

            var tblFeedback = await order
                .FirstOrDefaultAsync(m => m.orderID == id);
            if (tblFeedback == null)
            {
                return NotFound();
            }

            return View(tblFeedback);
        }





        private bool TblFeedbackExists(string id)
        {
            return (_context.TblFeedbacks?.Any(e => e.FeedbackId == id)).GetValueOrDefault();
        }
    }
}
