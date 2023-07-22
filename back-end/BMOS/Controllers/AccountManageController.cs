using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BMOS.Models.Entities;
using X.PagedList;

namespace BMOS.Controllers
{
	public class AccountManageController : Controller
	{
		private readonly BmosContext _context;

		public AccountManageController(BmosContext context)
		{
			_context = context;
		}

		// GET: UsersManage
		public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
		{
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

            var accounts = from s in _context.TblUsers
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                accounts = accounts.Where(s => s.Username.Contains(searchString));
                int count = accounts.Count();
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
                    accounts = accounts.OrderBy(s => s.Username);
                    break;
                case "name_desc":
                    accounts = accounts.OrderByDescending(s => s.Username);
                    break;
                case "date":
                    accounts = accounts.OrderBy(s => s.DateCreate);
                    break;
                case "date_desc":
                    accounts = accounts.OrderByDescending(s => s.DateCreate);
                    break;
                default:
                    accounts = accounts.OrderBy(s => s.UserId);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(accounts.ToPagedList(pageNumber, pageSize));
        }
    

		// GET: UsersManage/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.TblUsers == null)
			{
				return NotFound();
			}

			var tblUser = await _context.TblUsers
				.FirstOrDefaultAsync(m => m.UserId == id);
			if (tblUser == null)
			{
				return NotFound();
			}
			return View(tblUser);
		}

		// GET: UsersManage/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: UsersManage/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("UserId,Username,Password,IsConfirm,Firstname,Lastname,Numberphone,Address,DateCreate,LastActivitty,Point,Status,UserRoleId")] TblUser tblUser)
		{
			if (ModelState.IsValid)
			{
				_context.Add(tblUser);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(tblUser);
		}

		// GET: UsersManage/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.TblUsers == null)
			{
				return NotFound();
			}

			var tblUser = await _context.TblUsers.FindAsync(id);
			if (tblUser == null)
			{
				return NotFound();
			}
			return View(tblUser);
		}

		// POST: UsersManage/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Password,IsConfirm,Firstname,Lastname,Numberphone,Address,DateCreate,LastActivitty,Point,Status,UserRoleId")] TblUser tblUser)
		{
			if (id != tblUser.UserId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(tblUser);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TblUserExists(tblUser.UserId))
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
			return View(tblUser);
		}


		private bool TblUserExists(int id)
		{
			return (_context.TblUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
		}
	}
}
