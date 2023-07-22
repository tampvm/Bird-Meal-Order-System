using BMOS.Models;
using BMOS.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BMOS.Controllers
{
	public class BlogController : Controller
	{
		private readonly BmosContext _context;

		public BlogController(BmosContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Blog(string sortOrder, string currentFilter, string searchString, int? page)
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

			var blogs = from blog in _context.TblBlogs
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

		public async Task<IActionResult> BlogDetail(String id)
		{
			if (id == null || _context.TblBlogs == null)
			{
				return NotFound();
			}

			var TblBlogs = await _context.TblBlogs
				 .FirstOrDefaultAsync(m => m.BlogId == id);
			if (TblBlogs == null)
			{
				return NotFound();
			}
			return View(TblBlogs);
		}


	}
}
