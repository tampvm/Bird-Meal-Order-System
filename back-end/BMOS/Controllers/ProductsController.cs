using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BMOS.Models.Entities;
using BMOS.Models;
using BMOS.Models.Services;
using X.PagedList;

namespace BMOS.Controllers
{
    public class ProductsController : Controller
    {
        private static string ApiKey = "AIzaSyAYLSdMSB9rr3mF2WBNrTNVaxTdMPF_cjo";
        private static string Bucket = "bmos-4bc92.appspot.com";
        private static string AuthEmail = "lechiphat1909@gmail.com";
        private static string AuthPassword = "123456";

        private readonly BmosContext _context;

        public ProductsController(BmosContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Product(String id)
        {
            var userSession = HttpContext.Session.GetString("username");
            if (userSession != null)
            {
                ViewBag.ID = "Login";
            }
            var recom = _context.TblProducts.Where(x => x.ProductId.Equals(id)).FirstOrDefault();

            var _relatedProduct = _context.TblProducts.OrderByDescending(s => s.ProductId).Where(x => x.Type == recom.Type && x.ProductId != id).Take(3);
            var result = from img in _context.TblImages
                         from prod in _relatedProduct
                         where prod.ProductId == img.RelationId
                         select (new RelatedProductModel
                         {
                             _id = prod.ProductId,
                             _prodName = prod.Name,
                             _prodPrice = prod.Price,
                             _image = img.Url
                         });
            var _listProductRelated = result.ToList();


            var feedbackList = from feedback in _context.TblFeedbacks
                               from product in _context.TblProducts
                               from user in _context.TblUsers
                               where feedback.ProductId == product.ProductId && feedback.UserId == user.UserId && product.ProductId == id
                               select new FeedbackInfoModel
                               {
                                   fullName = user.Firstname + user.Lastname,
                                   ProductId = product.ProductId,
                                   FeedbackId = feedback.FeedbackId,
                                   UserId = user.UserId,
                                   Star = feedback.Star,
                                   Content = feedback.Content,
                                   date = feedback.Date,
                               };
            var averageStart = (from feedback in feedbackList where feedback.FeedbackId != null select feedback.Star).Average();
            ViewData["AverageStartFeedback"] = averageStart;
			ViewData["FeedbackQuantity"] = feedbackList.ToList().Count();
            ViewData["Feedback"] = feedbackList.ToList();




            var productItem = from product in _context.TblProducts
                              from image in _context.TblImages
                              where product.ProductId == image.RelationId
                              select new ProductInfoModel()
                              {
                                  ProductId = product.ProductId,
                                  Name = product.Name,
                                  Quantity = product.Quantity,
                                  Price = product.Price,
                                  Description = product.Description,
                                  Weight = product.Weight,
                                  IsLoved = product.IsLoved,
                                  SoldQuantity = product.SoldQuantity,
                                  Status = product.Status,
                                  UrlImage = image.Url,
                                  relatedProductModels = _listProductRelated
                              };

            var productDetail = await productItem.FirstOrDefaultAsync(item => item.ProductId.Equals(id));
            if (productDetail == null)
            {
                return NotFound();
            }
            return View(productDetail);
        }

        public async Task<IActionResult> ListProduct(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["SearchParameter"] = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "name" ? "" : "name";
			ViewData["NameDescSortParm"] = sortOrder == "name_desc" ? "" : "name_desc";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "" : "price";
            ViewData["PriceDescSortParm"] = sortOrder == "price_desc" ? "" : "price_desc";


			if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //var blogs = from blog in _context.TblBlogs
            //            from image in _context.TblImages
            //            where blog.BlogId == image.RelationId && blog.Status != false
            //            select new BlogInfoModel
            //            {
            //                blogId = blog.BlogId,
            //                blogName = blog.Name,
            //                blogDescription = blog.Description,
            //                blogImage = image.Url,
            //                Date = blog.Date,
            //            };
            //ViewData["BlogList"] = blogs;

            var products = from product in _context.TblProducts
                           from image in _context.TblImages
                           where product.ProductId.Equals(image.RelationId)
                           select new ProductInfoModel()
                           {
                               ProductId = product.ProductId,
                               Name = product.Name,
                               Price = product.Price,
                               Description = product.Description,
                               Weight = product.Weight,
                               IsLoved = product.IsLoved,
                               UrlImage = image.Url,
                               relatedProductModels = null
                           };
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
                int count = products.Count();
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
                    products = products.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "price":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductId);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> ThucAnHat(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["SearchParameter"] = searchString;
            ViewBag.CurrentSort = sortOrder;
			ViewData["NameSortParm"] = sortOrder == "name" ? "" : "name";
			ViewData["NameDescSortParm"] = sortOrder == "name_desc" ? "" : "name_desc";
			ViewData["PriceSortParm"] = sortOrder == "price" ? "" : "price";
			ViewData["PriceDescSortParm"] = sortOrder == "price_desc" ? "" : "price_desc";

			if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var products = from product in _context.TblProducts
                           from image in _context.TblImages
                           where product.ProductId.Equals(image.RelationId)
                           select new ProductInfoModel()
                           {
                               ProductId = product.ProductId,
                               Name = product.Name,
                               Price = product.Price,
                               Type = product.Type,
                               Description = product.Description,
                               Weight = product.Weight,
                               IsLoved = product.IsLoved,
                               UrlImage = image.Url,
                               relatedProductModels = null
                           };

            var result = products.Where(x => x.Type == "1").ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.Name.Contains(searchString)).ToList();
                int count = result.Count();
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
                    products = products.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "price":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductId);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> ThucAnKho(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["SearchParameter"] = searchString;
            ViewBag.CurrentSort = sortOrder;
			ViewData["NameSortParm"] = sortOrder == "name" ? "" : "name";
			ViewData["NameDescSortParm"] = sortOrder == "name_desc" ? "" : "name_desc";
			ViewData["PriceSortParm"] = sortOrder == "price" ? "" : "price";
			ViewData["PriceDescSortParm"] = sortOrder == "price_desc" ? "" : "price_desc";

			if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var products = from product in _context.TblProducts
                           from image in _context.TblImages
                           where product.ProductId.Equals(image.RelationId)
                           select new ProductInfoModel()
                           {
                               ProductId = product.ProductId,
                               Name = product.Name,
                               Price = product.Price,
                               Type = product.Type,
                               Description = product.Description,
                               Weight = product.Weight,
                               IsLoved = product.IsLoved,
                               UrlImage = image.Url,
                               relatedProductModels = null
                           };

            var result = products.Where(x => x.Type == "2").ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.Name.Contains(searchString)).ToList();
                int count = result.Count();
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
                    products = products.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "price":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductId);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }
        public async Task<IActionResult> ThucAnTuNhien(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["SearchParameter"] = searchString;
            ViewBag.CurrentSort = sortOrder;
			ViewData["NameSortParm"] = sortOrder == "name" ? "" : "name";
			ViewData["NameDescSortParm"] = sortOrder == "name_desc" ? "" : "name_desc";
			ViewData["PriceSortParm"] = sortOrder == "price" ? "" : "price";
			ViewData["PriceDescSortParm"] = sortOrder == "price_desc" ? "" : "price_desc";

			if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;



            var products = from product in _context.TblProducts
                           from image in _context.TblImages
                           where product.ProductId.Equals(image.RelationId)
                           select new ProductInfoModel()
                           {
                               ProductId = product.ProductId,
                               Name = product.Name,
                               Price = product.Price,
                               Type = product.Type,
                               Description = product.Description,
                               Weight = product.Weight,
                               IsLoved = product.IsLoved,

                               UrlImage = image.Url,
                               relatedProductModels = null
                           };

            var result = products.Where(x => x.Type == "3").ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.Name.Contains(searchString)).ToList();
                int count = result.Count();
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
                    products = products.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "price":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductId);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult ChangeLove(string id, string type)
        {
            var result = _context.TblProducts.FirstOrDefault(x => x.ProductId.Equals(id));
            result.IsLoved = !result.IsLoved;
            _context.SaveChanges();
            if (type == "ajax")
            {
                return Json(new
                {
                    love = result.IsLoved
                });
            }
            return RedirectToAction("Products", new
            {
                id
            });
        }

        public async Task<IActionResult> Index()
        {
            var products = _context.TblProducts.Where(p => p.IsLoved == true).ToList();


            return View(products);
        }

        public IActionResult Notify()
        {
            var user = HttpContext.Session.GetString("username");

            if (user != null)
            {

                var tb = from n in _context.TblNotifies select n;
                int q = tb.Count();
                if (q == 0)
                {
                    ViewBag.message = "Bạn không có thông báo nào!";
                }

                return View(tb);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public IActionResult Comment(string id, string textcontent, int starvalue)
        {
            var user = HttpContext.Session.GetString("username");
            var userid = _context.TblUsers.Where(p => p.Username.Equals(user)).Select(p => p.UserId).First();

            TblFeedback feedback = new TblFeedback();
            feedback.ProductId = id;
            feedback.Content = textcontent;
            feedback.Date = DateTime.Now;
            feedback.Star = starvalue;
            feedback.UserId = userid;
            feedback.FeedbackId = Guid.NewGuid().ToString();
            _context.Add(feedback);
            _context.SaveChanges();
            return RedirectToAction("Product", "Products", new
            {
                id
            });

        }

    }
}
