using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BMOS.Models.Entities;
using X.PagedList;
using BMOS.Models;
using Org.BouncyCastle.Asn1.X9;
using BMOS.Helpers;

namespace BMOS.Controllers
{
    public class RefundsManageController : Controller
    {
        private readonly BmosContext _context;

        public RefundsManageController(BmosContext context)
        {
            _context = context;
        }

        // GET: RefundsManage
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
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var refunds = from f in _context.TblRefunds
                          from o in _context.TblOrders
                          join u in _context.TblUsers on f.UserId equals u.UserId
                          where f.UserId.Equals(o.UserId) && f.OrderId.Equals(o.OrderId)
                          select new RefundsInfoModel()
                          {
                              RefundId = f.RefundId,
                              UserName = u.Firstname + u.Lastname,
                              UserId = o.UserId,
                              OrderId = o.OrderId,
                              Description = f.Description,
                              Date = f.Date,
                              IsConfirm = f.IsConfirm,  
                          };
            
            if (!String.IsNullOrEmpty(searchString))
            {
                refunds = refunds.Where(s => s.RefundId.Contains(searchString));
                int count = refunds.Count();
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
                case "date":
                    refunds = refunds.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    refunds = refunds.OrderByDescending(s => s.Date);
                    break;
                default:
                    refunds = refunds.OrderBy(s => s.RefundId);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(refunds.ToPagedList(pageNumber, pageSize));
        }

        // GET: RefundsManage/Details/5
        public async Task<IActionResult> Details(string id )
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
            var refunds = from f in _context.TblRefunds
                          from o in _context.TblOrders
                          join u in _context.TblUsers on f.UserId equals u.UserId
						  from image in _context.TblImages
						  where f.OrderId == image.RelationId && f.UserId.Equals(o.UserId) && f.OrderId.Equals(o.OrderId)
						  select new RefundsInfoModel()
                          {
                              RefundId = f.RefundId,
                              UserName = u.Firstname + u.Lastname,
                              UserId = o.UserId,
                              OrderId = o.OrderId,
                              Description = f.Description,
                              Date = f.Date,
                              IsConfirm = f.IsConfirm,
                              urlImage = image.Url
						  };
            
            if (id == null || _context.TblRefunds == null)
            {
                return NotFound();
            }
            
            var tblRefund = await refunds
                .FirstOrDefaultAsync(m => m.RefundId == id);
            var orderdetails = from d in _context.TblOrderDetails
                               join p in _context.TblProducts on d.ProductId equals p.ProductId
                               join o in refunds on d.OrderId equals o.OrderId
                               from image in _context.TblImages
                               where (d.OrderId == o.OrderId && p.ProductId.Equals(d.ProductId)) && (d.ProductId.Equals(image.RelationId))
                               select new OrderdetailsInfo()
                               {
                                   orderId = d.OrderId,
                                   namepro = p.Name,
                                   quantity = (int)d.Quantity,
                                   price = (double)(d.Price * d.Quantity),
                                   urlImage = image.Url,
                               };
            var infoorder = orderdetails.Where(m => m.orderId == tblRefund.OrderId).ToList();
            ViewData["OrderDetails"] = infoorder.ToList();
            if (tblRefund == null)
            {
                return NotFound();
            }

            return View(tblRefund);
        }

		

		// GET: RefundsManage/Edit/5
		public async Task<IActionResult> Edit(string id)
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
            if (id == null || _context.TblRefunds == null)
            {
                return NotFound();
            }
            ViewBag.Confirm = new List<string>() {"Chưa xác nhận đơn hàng", "Xác nhận đơn hàng"};

            var refunds = from f in _context.TblRefunds
                          from o in _context.TblOrders
                          join u in _context.TblUsers on f.UserId equals u.UserId
                          where f.UserId.Equals(o.UserId) && f.OrderId.Equals(o.OrderId)
                          select new RefundsInfoModel()
                          {
                              RefundId = f.RefundId,
                              UserName = u.Firstname + u.Lastname,
                              UserId = o.UserId,
                              OrderId = o.OrderId,
                              Description = f.Description,
                              Date = f.Date,
                              IsConfirm = f.IsConfirm,
                          };

            if (id == null || _context.TblRefunds == null)
            {
                return NotFound();
            }

            var tblRefund = await refunds
                .FirstOrDefaultAsync(m => m.RefundId == id);
            
            if (tblRefund == null)
            {
                return NotFound();
            }
            return View(tblRefund);
        }

        // POST: RefundsManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RefundId,UserId,OrderId,Description,Date,IsConfirm")] TblRefund tblRefund)
        {
            if (id != tblRefund.RefundId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var order = _context.TblOrders.Where(x => x.OrderId.Equals(tblRefund.OrderId)).FirstOrDefault();
                    _context.Update(tblRefund);
					if (tblRefund.IsConfirm == true)
					{
                        order.Status = 6;
                        TblNotify notify = new TblNotify();
						{
							notify.NotifyId = Guid.NewGuid().ToString();
							notify.UserId = tblRefund.UserId;
							notify.Date = tblRefund.Date;
							notify.Type = "refund";
							notify.Message = "Đơn hàng " + tblRefund.OrderId + " của bạn đã được xác nhận hoàn trả";
							_context.Update(order);
							_context.Add(notify);
                            _context.SaveChanges();
						}
					}if (tblRefund.IsConfirm == false)
                    {
                        order.Status = 5;
                        TblNotify notify = new TblNotify();
						{
							notify.NotifyId = Guid.NewGuid().ToString();
							notify.UserId = tblRefund.UserId;
							notify.Date = tblRefund.Date;
							notify.Type = "refund";
							notify.Message = "Đơn hàng " + tblRefund.OrderId + " của bạn đã bị từ chối";
							_context.Add(notify);
                            _context.Update(order);
                            _context.SaveChanges();
						}
					}
					await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblRefundExists(tblRefund.RefundId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tblRefund);
        }

        
        private bool TblRefundExists(string id)
        {
          return (_context.TblRefunds?.Any(e => e.RefundId == id)).GetValueOrDefault();
        }
    }
}
