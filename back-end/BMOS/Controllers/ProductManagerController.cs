using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BMOS.Models.Entities;
using BMOS.Models.Services;
using X.PagedList;
using BMOS.Models;

namespace BMOS.Controllers
{
    public class ProductManagerController : Controller
    {
        private readonly BmosContext _context;

        public ProductManagerController(BmosContext context)
        {
            _context = context;
        }

        // GET: ProductManager
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

            var productwithImage = from product in _context.TblProducts.ToList()
                                   from image in _context.TblImages.ToList()
                         where product.ProductId.Equals(image.RelationId)
                         select new ProductInfoModel
                         {
                             ProductId = product.ProductId,
                             Name = product.Name,
                             UrlImage = image.Url,
                             Quantity = product.Quantity,
                             Status = product.Status,
                             Price = product.Price,
                             SoldQuantity = product.SoldQuantity,
                             Weight = product.Weight,
                         };
            var feedback = productwithImage;
			if (!String.IsNullOrEmpty(searchString))
            {
                feedback = feedback.Where(s => s.Name.Contains(searchString));
                int count = feedback.Count();
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
                    feedback = feedback.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    feedback = feedback.OrderByDescending(s => s.Name);
                    break;
                case "price":
                    feedback = feedback.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    feedback = feedback.OrderByDescending(s => s.Price);
                    break;
                default:
                    feedback = feedback.OrderBy(s => s.ProductId);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(feedback.ToPagedList(pageNumber, pageSize));
            //return View(feedback.ToList());
        }

		// GET: ProductManager/Details/5
		public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TblProducts == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProducts
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (tblProduct == null)
            {
                return NotFound();
            }

            return View(tblProduct);
        }

        // GET: ProductManager/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductManager/Create
        // To protect from overposting attacks,  enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name, Quantity, Price, Description,Weight,IsAvailable,IsLoved,Status,Type")] TblProduct tblProduct, List<IFormFile> files)
        {

            string url = "";
            if (ModelState.IsValid)
            {
                url = await FirebaseService.UploadImage(files, "products");
                tblProduct.IsLoved = false;
                _context.Add(tblProduct);
                TblImage tblImage = new TblImage
                {
                    ImageId = Guid.NewGuid().ToString(),
                    Name = "Product img",
                    RelationId = tblProduct.ProductId,
                    Type = "Product",
                    Url = url
                };
                _context.TblImages.Add(tblImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(tblProduct);
        }

        // GET: ProductManager/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TblProducts == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProducts.FindAsync(id);
            if (tblProduct == null)
            {
                return NotFound();
            }
            return View(tblProduct);
        }

        // POST: ProductManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProductId,Name,Quantity,Description,Weight,IsAvailable,IsLoved,Status,Price,ImagelInk")] TblProduct tblProduct)
        {
            if (id != tblProduct.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblProductExists(tblProduct.ProductId))
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
            return View(tblProduct);
        }   

        // GET: ProductManager/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.TblProducts == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProducts
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (tblProduct == null)
            {
                return NotFound();
            }

            return View(tblProduct);
        }

        // POST: ProductManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TblProducts == null)
            {
                return Problem("Entity set 'BmosContext.TblProducts'  is null.");
            }
            var tblProduct = await _context.TblProducts.FindAsync(id);
            if (tblProduct != null)
            {
                _context.TblProducts.Remove(tblProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblProductExists(string id)
        {
            return (_context.TblProducts?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
