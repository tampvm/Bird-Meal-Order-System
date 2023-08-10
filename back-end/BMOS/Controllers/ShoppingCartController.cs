using BMOS.Helpers;
using BMOS.Models;
using BMOS.Models.Entities;
using BMOS.Services;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.DevTools.V112.DOM;
using System.Drawing;
using System.Net.WebSockets;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;


namespace BMOS.Controllers
{
	public class ShoppingCartController : Controller
	{

		private readonly BmosContext _context;
		public ShoppingCartController(BmosContext context)
		{
			_context = context;

		}

		public double? getTotalCartPrice()
		{
			double? result = 0;
			var myCart = Cart;
			foreach (var item in myCart.ToList())
			{
				result += item._getTotalPrice();
			}
			return result;
		}

		public List<CartModel> Cart
		{
			get
			{
				var data = HttpContext.Session.Get<List<CartModel>>("Cart");
				if (data == null)
				{
					data = new List<CartModel>();
				}
				return data;
			}
			set
			{

			}
		}

		public IActionResult AddProductInRoutingToCart()
		{
			var user = HttpContext.Session.Get<TblUser>("user");
			if (user != null)
			{
				if (user.UserRoleId == 2 || user.UserRoleId == 1)
				{
					return View("ErrorPage");
				}
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
			var myCart = Cart;
			var routingList = HttpContext?.Session.Get<List<RoutingDetailModel>>("Routing");

			// modifier cart empty
			foreach (var _rprod in routingList)
			{
				if (myCart.Count == 0)
				{
					var _product = _context.TblProducts.SingleOrDefault(p => p.ProductId.Equals(_rprod.productId));
					var _productImge = _context.TblImages.FirstOrDefault(x => x.RelationId.Equals(_rprod.productId))?.Url;

					if (_productImge != null)
					{
						string[] delemeter = new string[] { "datnt" };
						string[] image = _productImge.Split(delemeter, StringSplitOptions.None);
						myCart.Add(new CartModel
						{
							_productId = _rprod.productId,
							_productName = _rprod.productName,
							_quantity = _rprod.productQuantity,
							productAvailable = _rprod.quantity,
							sold = _rprod.sold,
							_price = _rprod.productPrice,
							_weight = _product.Weight,
							_productImage = image[0]
						});
					}
					// cart not empty
				}
				else
				{
					var item = myCart.SingleOrDefault(p => p._productId.Equals(_rprod.productId));
					if (item != null)
					{
						item._quantity += _rprod.productQuantity;
						if (item._quantity >= item.getProductAvailable())
						{
							item._quantity = item.getProductAvailable();
						}
					}
					else
					{
						var _product = _context.TblProducts.SingleOrDefault(p => p.ProductId.Equals(_rprod.productId));
						var _productImge = _context.TblImages.FirstOrDefault(x => x.RelationId.Equals(_rprod.productId))?.Url;

						if (_productImge != null)
						{
							string[] delemeter = new string[] { "datnt" };
							string[] image = _productImge.Split(delemeter, StringSplitOptions.None);
							myCart.Add(new CartModel
							{
								_productId = _rprod.productId,
								_productName = _rprod.productName,
								_quantity = _rprod.productQuantity,
								productAvailable = _rprod.quantity,
								sold = _rprod.sold,
								_price = _rprod.productPrice,
								_weight = _product.Weight,
								_productImage = image[0]
							});
						}
					}
				}
			}

			HttpContext?.Session.Set("Cart", myCart);
			return RedirectToAction("Index");
		}

		public IActionResult useBonusCode(string code)
		{
			var myCart = Cart;

			var user = HttpContext.Session.Get<TblUser>("user");
			double? totalPrice = 0;
			double? resultPrice = 0;
			foreach (var item in myCart.ToList())
			{
				totalPrice += item._getTotalPrice();
			}


			if (totalPrice >= 100000)
			{
				var checkVoucher = _context.TblVoucherCodes.Where(x => x.VoucherCode.Equals(code)).FirstOrDefault();

				//check code da su dung
				// lay userid ra. tra trong bang co voucherID do chua
				var checkVoucherUsed = _context.TblVoucherUseds.Where(x => x.UserId.Equals(user.UserId)).Select(x => x.VoucherId).FirstOrDefault();

				if (checkVoucher != null)
				{
					if (checkVoucher.VoucherId.Equals(checkVoucherUsed))
					{
						TempData["errorVoucher"] = "Code này đã được sử dụng! Bạn vui lòng thử lại";
						return RedirectToAction("UpdateCart");
					} // check code da duoc su dung

					if (checkVoucher.Quantity == checkVoucher.Used)
					{
						TempData["errorVoucher"] = "Code này đã hết lượt sử dụng! Bạn vui lòng thử lại mã khác";
						return RedirectToAction("UpdateCart");
					} // check code đã hết


					resultPrice = totalPrice - checkVoucher.Value;
					HttpContext.Session.Set("voucherInput", code);
					HttpContext.Session.Set("useVoucher", true);
					HttpContext.Session.Set("voucherId", checkVoucher.VoucherId);
					HttpContext.Session.Set("resultPrice", resultPrice);
					HttpContext.Session.Set("discountPrice", checkVoucher.Value);
				}
				else
				{
					TempData["errorVoucher"] = "Code bạn sử dụng không hợp lệ!";
				}
			}
			else
			{
				TempData["errorVoucher"] = "Giá trị đơn hàng phải từ 100.000vnd trở lên mới có thể sử dụng Voucher";
			}//unvalid totalpricce for use point
			HttpContext.Session.Set("resultPrice", resultPrice);
			return RedirectToAction("UpdateCart");
		}
		public IActionResult useBonusPoint(double point)
		{
			var myCart = Cart;

			var user = HttpContext.Session.Get<TblUser>("user");
			var userPoint = user.Point;

			double? totalPrice = 0;
			foreach (var item in myCart.ToList())
			{
				totalPrice += item._getTotalPrice();
			}
			double? resultPrice = 0;
			if (userPoint != null)
			{
				if (userPoint < point)
				{
					TempData["errorPoint"] = "Điểm tích lũy của bạn không phù hợp. Vui lòng thử lại";
					return RedirectToAction("UpdateCart");
				};
				//check userpoint 

				//validate price >= 100k
				if (totalPrice >= 100000)
				{
					//validate point >= 1000d && <= 10000d	
					if (point >= 1000 && point <= 10000)
					{
						resultPrice = totalPrice - (point * 10);

						HttpContext.Session.Set("resultPrice", resultPrice);
						HttpContext.Session.Set("discountPrice", point * 10);
						HttpContext.Session.Set("pointInput", point);

						//user.Point = userPoint - point;
						//_context.Update(user);
					}
					else
					{
						TempData["errorPoint"] = "Điểm sử dụng giới hạn từ 1.000 - 10.000d. Bạn vui lòng thử lại!";
					}// unvalid point uses
				}
				else
				{
					TempData["errorPoint"] = "Giá trị đơn hàng phải từ 100.000vnd trở lên mới có thể sử dụng điểm";
				}//unvalid totalpricce for use point

			}
			HttpContext.Session.Set("resultPrice", resultPrice);
			return RedirectToAction("UpdateCart");
		}

		public IActionResult Checkout()
		{
			var user = HttpContext.Session.Get<TblUser>("user");
			if (user != null) { RedirectToAction("Index"); }
			var discountPrice = HttpContext.Session.Get<double>("discountPrice");

			var totalPriceBeforeUseDiscount = getTotalCartPrice();
			if (user == null) { return RedirectToAction("Login", "Account"); }
			var _priceProduct = getTotalCartPrice();
			var Address = _context.TblAddresses.Where(x => x.UserId == user.UserId).FirstOrDefault();
			decimal? bonusPoint = 0;

			if (_priceProduct >= 100000)
			{
				bonusPoint = Math.Round((decimal)_priceProduct / 1000, MidpointRounding.AwayFromZero);
			}
			var usePointPrice = HttpContext.Session.Get<double>("resultPrice");
			if (usePointPrice > 0)
			{
				_priceProduct = usePointPrice;
			}

			//cofig list address here...............
			ViewData["Address"] = Address;
			ViewData["totalPriceBeforeUseDiscount"] = totalPriceBeforeUseDiscount;
			ViewData["discountPrice"] = discountPrice;
			ViewData["totalPrice"] = _priceProduct;
			ViewData["bonusPoint"] = bonusPoint;
			ViewData["Cart"] = Cart;
			HttpContext.Session.Remove("resultPrice");
			// update user point.
			// use point feature --ok
			return View(user);
		}

		public async Task<IActionResult> ConfirmOrder(string userId, string orderId, double point = 0)
		{

			var myCart = Cart;
			userId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userId));
			var order = await _context.TblOrders.Where(o => o.OrderId == orderId).FirstOrDefaultAsync();
			order.IsConfirm = true;
			order.Status = 1;
			_context.Update(order);

			var user = await _context.TblUsers.Where(u => u.Username.Equals(userId)).FirstOrDefaultAsync();
			//double? currentPoint = user.Point;
			//currentPoint += point;
			//user.Point = currentPoint;
			//_context.Update(user);
			var orderDetailNum = _context.TblOrderDetails.Count(x => x.OrderDetailId != null);
			foreach (var item in myCart.ToList())
			{
				var product = _context.TblProducts.Where(x => x.ProductId.Equals(item._productId)).FirstOrDefault();
				if (product != null)
				{
					var soldQuantity = product.SoldQuantity;
					soldQuantity += item._quantity;
					product.SoldQuantity = soldQuantity;
					_context.Update(product);
				}
			}

			_context.SaveChanges();
			HttpContext.Session.Set("confirmOrderStatus", true);

			myCart.Clear();
			HttpContext?.Session.Set("Cart", myCart);
			return RedirectToAction("Index", "Home");
		}
		[HttpPost]
		public async Task<IActionResult> Payment(string address, string phone, string note, string pType = "COD", double point = 0)
		{
			var user = HttpContext.Session.Get<TblUser>("user");
			var usePointPrice = HttpContext.Session.Get<double>("resultPrice");
			var voucherCode = HttpContext.Session.Get<string>("voucherId");
			var cartPrice = getTotalCartPrice();
			var cartQuantity = Cart.Count();
			if (usePointPrice > 0)
			{
				cartPrice = usePointPrice;
			}

			if (user == null) { return RedirectToAction("Login", "Account"); }
			var myCart = Cart;
			var orderNum = _context.TblOrders.Count(x => x.OrderId != null);
			var orderDetailNum = _context.TblOrderDetails.Count(x => x.OrderDetailId != null);
			var orderId = "order" + orderNum;
			var order = new TblOrder
			{
				OrderId = orderId,
				UserId = user.UserId,
				TotalPrice = cartPrice,
				Date = DateTime.Now,
				IsConfirm = false,
				Address = address,
				Phone = phone,
				Note = note,
				PaymentType = pType,
				Point = point,
				Status = 0,
			};
			_context.Add(order);

			foreach (var item in myCart.ToList())
			{
				var orderDetail = new TblOrderDetail
				{
					OrderDetailId = "orderdetail" + orderDetailNum,
					OrderId = order.OrderId,
					ProductId = item._productId,
					Quantity = item._quantity,
					Price = item._price,
					Date = DateTime.Now,
				};
				orderDetailNum++;
				_context.Add(orderDetail);

			}

			//add to voucehr used
			var isUsedVoucher = HttpContext.Session.Get<bool>("useVoucher");
			if (isUsedVoucher)
			{
				var voucherUsedId = _context.TblVoucherUseds.Count(x => x.VoucherId != null);
				var voucherUsed = new TblVoucherUsed
				{
					Id = voucherUsedId,
					UserId = user.UserId,
					VoucherId = voucherCode,
				};
				_context.Add(voucherUsed);
				var voucher = _context.TblVoucherCodes.Where(x => x.VoucherId.Equals(voucherCode)).FirstOrDefault();
				if (voucher != null)
				{
					voucher.Used++;
					_context.Update(voucher);
				}
			}

			_context.SaveChanges();
			var userId = user.Username;
			userId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(userId));
			if (userId != null)
			{
				var fullName = user.Firstname + " " + user.Lastname;
				var username = user.Username;
				var content = Url.Action("ConfirmOrder", "ShoppingCart", new { userId = userId, orderId = orderId, point = point }, protocol: Request.Scheme);
				await EmailSender.SendEmailConfirmOrderAsync(username, "Bạn có đơn hàng trên BMOS", "<a href=\"" + content + "\" class=\"linkdetail\" style=\"text-decoration: none; margin: 0 auto; color: black;\">Xác nhận</a>"
					, cartQuantity, cartPrice, order, fullName);
			}

			return View();
		}





		// handle with cart
		public IActionResult RemoveItem(string id)
		{
			var myCart = Cart;
			foreach (var item in myCart.ToList())
			{
				if (item._productId.Equals(id))
				{
					myCart.Remove(item);
				}
			}
			HttpContext?.Session.Set("Cart", myCart);
			return RedirectToAction("UpdateCart");

		}

		public IActionResult UpdateCart(string id, string status, int productQuantity = 1)
		{
			double? totalPrice = 0;
			var myCart = Cart;

			foreach (var item in myCart.ToList())
			{
				if (item._productId.Equals(id))
				{
					if (status.Equals("increase"))
					{
						item._quantity += productQuantity;
						if (item._quantity >= item.getProductAvailable())
						{
							item._quantity = item.getProductAvailable();
						}
					}
					else if (status.Equals("decrease"))
					{
						item._quantity -= productQuantity;
						if (item._quantity == 0)
						{
							myCart.Remove(item);
						}
					}
				}
				totalPrice += item._getTotalPrice();
			}
			var user = HttpContext.Session.Get<TblUser>("user");
			var resultPrice = HttpContext.Session.Get<double>("resultPrice");
			var pointInput = HttpContext.Session.Get<double>("pointInput");
			var voucherInput = HttpContext.Session.Get<string>("voucherInput");

			double? userPoint = user.Point;
			decimal? bonusPoint = 0;
			if (totalPrice >= 100000)
			{
				bonusPoint = Math.Round((decimal)totalPrice, MidpointRounding.AwayFromZero);
			}
			//ViewBag.errorPointUse = TempData["errorPoint"];
			ViewData["resultPrice"] = resultPrice;
			ViewData["userPoint"] = userPoint;
			ViewData["total"] = totalPrice;
			ViewData["bonusPoint"] = bonusPoint;
			ViewData["pointInput"] = pointInput;
			ViewData["voucherInput"] = voucherInput;

			HttpContext?.Session.Set("Cart", myCart);
			HttpContext?.Session.Remove("pointInput");
			HttpContext?.Session.Remove("voucherInput");
			return PartialView(myCart);

		}

		public IActionResult AddToCart(string id, int productQuantity = 1, string type = "Normal")
		{
			var user = HttpContext.Session.Get<TblUser>("user");
			if (user != null)
			{
				if (user.UserRoleId == 2 || user.UserRoleId == 1)
				{
					return View("ErrorPage");
				}
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
			var myCart = Cart;
			var item = myCart.SingleOrDefault(p => p._productId.Equals(id));
			var _loginStatus = HttpContext.Session.Get<TblUser>("user") != null ? true : false;
			if (item == null)
			{
				var _product = _context.TblProducts.SingleOrDefault(p => p.ProductId.Equals(id));
				var _productImge = _context.TblImages.FirstOrDefault(x => x.RelationId.Equals(id))?.Url;

				if (_productImge != null)
				{
					string[] delemeter = new string[] { "datnt" };
					string[] image = _productImge.Split(delemeter, StringSplitOptions.None);
					item = new CartModel()
					{
						_productId = id,
						_productName = _product.Name,
						_quantity = productQuantity,
						_weight = _product.Weight,
						_price = _product.Price,
						sold = _product.SoldQuantity,
						productAvailable = _product.Quantity,
						_productImage = image[0],
					};
				}
				myCart.Add(item);
			}
			else
			{
				item._quantity += productQuantity;
			}
			if (_loginStatus) HttpContext?.Session.Set("Cart", myCart);
			if (type == "ajax")
			{
				return Json(new
				{
					productId = id,
					productQuantity = myCart.Count(),
					loginStatus = _loginStatus,
				});
			}
			return RedirectToAction("Index");

		}
		// GET: ShoppingCartController
		public IActionResult Index()
		{
			// remove session totalPrice when use bonuspoint and voucher
			HttpContext.Session.Remove("resultPrice");
			HttpContext.Session.Remove("discountPrice");
			double? userPoint = 0;

			var user = HttpContext.Session.Get<TblUser>("user");
			if (user != null)
			{
				if (user.UserRoleId == 2 || user.UserRoleId == 1)
				{
					return View("ErrorPage");
				}
				userPoint = user.Point;
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
			var _priceProduct = getTotalCartPrice();
			decimal? bonusPoint = 0;
			if (_priceProduct >= 100000)
			{
				bonusPoint = Math.Round((decimal)_priceProduct, MidpointRounding.AwayFromZero);
			}
			ViewData["userPoint"] = userPoint;
			ViewData["total"] = _priceProduct;
			ViewData["bonusPoint"] = bonusPoint;
			return View(Cart);



		}

	}
}
