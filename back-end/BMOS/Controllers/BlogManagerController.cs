using BMOS.Models;
using BMOS.Models.Entities;
using BMOS.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Policy;
using X.PagedList;

namespace Demo.Controllers
{
	public class BlogManagerController : Controller
	{
		private readonly BmosContext _context;

		public BlogManagerController(BmosContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
		{
			ViewData["SearchParameter"] = searchString;
			ViewBag.CurrentSort = sortOrder;
			ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name" : "";
			ViewData["NameDescSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date" : "";
			ViewData["DateDescSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var blogs = from blog in _context.TblBlogs.ToList()
						from image in _context.TblImages.ToList()
									   where blog.BlogId == image.RelationId && blog.Status != false
									   select new BlogInfoModel
									   {
										   blogId = blog.BlogId,
										   blogName = blog.Name,
										   blogDescription = blog.Description,
										   blogImage = image.Url,
										   Date = blog.Date,
									   };
			if (!String.IsNullOrEmpty(searchString))
			{
				blogs = blogs.Where(s => s.blogName.Contains(searchString));
				int count = blogs.Count();
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
					blogs = blogs.OrderBy(s => s.blogName);
					break;
				case "name_desc":
					blogs = blogs.OrderByDescending(s => s.blogName);
					break;
				case "date":
					blogs = blogs.OrderBy(s => s.Date);
					break;
				case "date_desc":
					blogs = blogs.OrderByDescending(s => s.Date);
					break;
				default:
					blogs = blogs.OrderBy(s => s.blogId);
					break;
			}
			int pageSize = 8;
			int pageNumber = (page ?? 1);
			return View(blogs.ToPagedList(pageNumber, pageSize));
		}
		public IActionResult Create()
		{
			return View();
		}
		public async Task<IActionResult> Details(string? id)
		{
			if (id == null || _context.TblBlogs == null)
			{
				return NotFound();
			}

			var tblBlog = await _context.TblBlogs
				.FirstOrDefaultAsync(m => m.BlogId == id);
			if (tblBlog == null)
			{
				return NotFound();
			}

			return View(tblBlog);
		}
		[HttpPost]
		public async Task<IActionResult> Create([Bind("BlogId, Name, Description, Date, Status")] TblBlog tblBlog, List<IFormFile> files)
		{
			string url = "";
			if (ModelState.IsValid)
			{
				url = await FirebaseService.UploadImage(files, "blogs");
				_context.Add(tblBlog);
				TblImage tblImage = new TblImage
				{
					ImageId = Guid.NewGuid().ToString(),
					Name = "Blog img",
					RelationId = tblBlog.BlogId,
					Type = "Blog",
					Url = url
				};
				_context.Add(tblImage);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(tblBlog);
		}
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null || _context.TblBlogs == null)
			{
				return NotFound();
			}

			var tblBlog = await _context.TblBlogs.FindAsync(id);
			if (tblBlog == null)
			{
				return NotFound();
			}
			return View(tblBlog);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("BlogId, Name, Description, Date, Status")] TblBlog tblBlog)
		{
			if (id != tblBlog.BlogId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(tblBlog);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TblBlogExists(tblBlog.BlogId))
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
			return View(tblBlog);
		}
		private bool TblBlogExists(string blogId)
		{
			throw new NotImplementedException();
		}
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null || _context.TblBlogs == null)
			{
				return NotFound();
			}

			var tblUser = await _context.TblBlogs
				.FirstOrDefaultAsync(m => m.BlogId == id);
			if (tblUser == null)
			{
				return NotFound();
			}

			return View(tblUser);
		}
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			if (_context.TblBlogs == null)
			{
				return Problem("Entity set 'BmosContext.TblBlogs'  is null.");
			}
			var tblBlog = await _context.TblBlogs.FindAsync(id);
			if (tblBlog != null)
			{
				_context.TblBlogs.Remove(tblBlog);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
	}
}
