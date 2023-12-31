﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using BMOS.Models.Entities;
using BMOS.Services;
using Microsoft.AspNetCore.Http;
using Firebase.Auth;
using Microsoft.EntityFrameworkCore;
using BMOS.Models;
using BMOS.Helpers;
using X.PagedList;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Google.Apis.Oauth2.v2.Data;
using Microsoft.Extensions.Caching.Memory;
using BMOS.Models.Services;

namespace BMOS.Controllers
{
	public class AccountController : Controller
	{
		private BmosContext _db = new BmosContext();
		private readonly IMemoryCache _cache;
		//private readonly GoogleAuthService _googleAuthService;
		////private readonly IHttpContextAccessor _httpContextAccessor;

		//public AccountController(GoogleAuthService googleAuthService)
		//{
		//    _googleAuthService = googleAuthService;
		//    //_httpContextAccessor = httpContextAccessor;
		//}

		public AccountController(IMemoryCache memoryCache)
		{
			_cache = memoryCache;
		}

		public IActionResult Login()
		{
			var user = HttpContext.Session.GetString("username");
			ViewBag.Confirmed = HttpContext.Session.GetString("notice");
			HttpContext.Session.Remove("notice");
            ViewBag.InvalidCode = HttpContext.Session.GetString("noticecode");
            HttpContext.Session.Remove("noticecode");
            ViewBag.InvalidCode1 = "để gửi lại mã xác nhận.";
            ViewBag.InvalidCode2 = HttpContext.Session.GetString("noticeincode");
            HttpContext.Session.Remove("noticeincode");
            ViewBag.IsConfirmed = HttpContext.Session.GetString("noticeisconfirm");
            HttpContext.Session.Remove("noticeisconfirm");
            ViewBag.ReSend = HttpContext.Session.GetString("noticeReSend");
            HttpContext.Session.Remove("noticeReSend");

			if (user != null)
			{
				return RedirectToAction("UserProfile");
			}
			return View();
		}

		[HttpPost]
		public IActionResult Login(TblUser model)
		{
			//if (ModelState.IsValid){
			string username = model.Username;
			string password = model.Password;

			if (username != null || password != null)
			{
				var check = _db.TblUsers.Where(p => p.Username.Equals(username) && p.Password.Equals(password)).Select(p => p.UserRoleId);
				if (check.Count() > 0)
				{
					var userManager = _db.TblUsers.Where(p => p.Username.Equals(username) && p.Password.Equals(password)).First();

					var checkStatus = _db.TblUsers.Where(p => p.Username.Equals(username) && p.Status == true);
					var id = check.First();
					//string sid = Convert.ToString(id);
					HttpContext.Session.SetString("id", id.ToString());
					if (id == 1)
					{
						HttpContext.Session.Set("userManager", userManager);
						HttpContext.Session.SetString("username", username);
						return RedirectToAction("Index", "Dashboard");
					}
					else if (id == 2 && checkStatus.Count() > 0)
					{
						HttpContext.Session.Set("userManager", userManager);
						HttpContext.Session.SetString("username", username);
						return RedirectToAction("Index", "ProductManager");
					}
                    else if (id == 4 && checkStatus.Count() > 0)
                    {
                        HttpContext.Session.Set("userManager", userManager);
                        HttpContext.Session.SetString("username", username);
                        return RedirectToAction("Index", "Delivery");
                    }
                    else if (id == 3 && checkStatus.Count() > 0)
					{
						var checkConfirm = _db.TblUsers.Where(p => p.Username.Equals(username) && p.IsConfirm == true).ToList();
						//string fullname = _db.TblUsers.Where(p => p.Username.Equals(username)).Select(p => p.Firstname).First() + " " + _db.TblUsers.Where(p => p.Username.Equals(username)).Select(p => p.Lastname).First();
						var user = _db.TblUsers.Where(p => p.Username.Equals(username)).First();
						string fullname = user.Firstname + " " + user.Lastname;

						if (checkConfirm.Count() > 0)
						{
							HttpContext.Session.SetString("username", username);
							HttpContext.Session.SetString("fullname", fullname);
							HttpContext.Session.Set("user", user);
							return RedirectToAction("Index", "Home");
						}
						ViewBag.EmailConfirm = "*Tài khoản của bạn chưa được kích hoạt, vui lòng kiểm tra Email để xác nhận tài khoản.";
						return View();
					}
					ViewBag.Block = "*Tài khoản của bạn đã bị khóa.";
					return View();
				}
				ViewBag.Notice = "*Thông tin đăng nhập không chính xác.";
				return View();
			}
			//}
			return View();
		}

		public IActionResult Logout()
		{
			var username = HttpContext.Session.GetString("username");
			var check = _db.TblUsers.Where(p => p.Username.Equals(username)).First();
			if (check != null)
			{
				check.LastActivitty = DateTime.Now;
				_db.SaveChanges();
			}
			HttpContext.Session.Remove("username");
			HttpContext.Session.Remove("fullname");
			HttpContext.Session.Remove("user");
			HttpContext.Session.Remove("email");
			return RedirectToAction("Index", "Home");
		}

		public IActionResult Register()
		{
			var user = HttpContext.Session.GetString("username");
			if (user != null)
			{
				return RedirectToAction("UserProfile");
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> RegisterAsync(TblUser model)
		{
			var userId = model.Username;
			var code = GenerateVerificationCode();
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			_cache.Set(userId, code, TimeSpan.FromMinutes(10));


			//var code = "qwert";
			//code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			userId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(userId));
			var content = Url.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: Request.Scheme);
			var check = _db.TblUsers.FirstOrDefault(p => p.Username == model.Username);

			if (check == null)
			{
				_db.TblUsers.Add(model);
				await EmailSender.SendEmailAsync(model.Username, "Xác thực tài khoản", "<a href=\"" + content + "\" class=\"linkdetail\" style=\"text-decoration: none; margin: 0 auto; color: black;\">Kích hoạt tài khoản</a>");
				_db.SaveChanges();

				HttpContext.Session.SetString("email", model.Username);

				ViewBag.RegisterSuccess = "*Đăng ký tài khoản thành công, vui lòng kiểm tra ";
				return View();
			}
			else
			{
				ViewBag.Error = "*Email đã được đăng ký.";
				return View();
			}
		}


		public async Task<IActionResult> ReSendEmail()
		{
			var email = HttpContext.Session.GetString("email");
			if (email != null)
			{
				var code = GenerateVerificationCode();
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
				_cache.Set(email, code, TimeSpan.FromMinutes(10));
				var email1 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(email));

				var content = Url.Action("ConfirmEmail", "Account", new { userId = email1, code = code }, protocol: Request.Scheme);

                await EmailSender.SendEmailAsync(email, "Xác thực tài khoản", "<a href=\"" + content + "\" class=\"linkdetail\" style=\"text-decoration: none; margin: 0 auto; color: black;\">Kích hoạt tài khoản</a>");
                string notice = "Hệ thống đã gửi lại mã xác nhận, vui lòng kiểm tra ";
                HttpContext.Session.SetString("noticeReSend", notice);
            }
            return RedirectToAction("Login");
        }


		public IActionResult ConfirmEmailAsync(string userId, string code)
		{
			userId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userId));
			var user = HttpContext.Session.GetString("username");
			string cachedVerificationCode = _cache.Get<string>(userId);
			if (user != null)
			{
				return RedirectToAction("UserProfile");
			}
			if (userId != null && cachedVerificationCode == code)
			{
                var check = _db.TblUsers.FirstOrDefault(p => p.Username == userId);
                if (check != null)
                {
                    check.IsConfirm = true;
                    _db.SaveChanges();
                    var notice = "*Cảm ơn bạn đã đăng ký tài khoản.";
                    HttpContext.Session.SetString("notice", notice);
                }
                return RedirectToAction("Login");
            }
			else if (userId != null && cachedVerificationCode == null)
			{
				var check = _db.TblUsers.FirstOrDefault(p => p.Username == userId && p.IsConfirm == true);
				if (check != null)
				{
					string isConfirmed = "Tài khoản của bạn đã được xác thực trước đó.";
					HttpContext.Session.SetString("noticeisconfirm", isConfirmed);
				}
				else
				{
                    string invalidCode = "Mã xác nhận đã hết thời gian hiệu lực, nhấn ";
                    HttpContext.Session.SetString("noticecode", invalidCode);
                }				
                return RedirectToAction("Login");
            }
			else
			{
				string invalid = "Mã xác nhận không tồn tại.";
                HttpContext.Session.SetString("noticeincode", invalid);
                return RedirectToAction("Login");
            }
			
		}

		private string GenerateVerificationCode()
		{
			// Tạo mã xác minh ngẫu nhiên.
			string verificationCode = "";

			Random random = new Random();
			for (int i = 0; i < 6; i++)
			{
				verificationCode += random.Next(0, 9);
			}

			return verificationCode;
		}

		public IActionResult ForgotPassword()
		{
            //var user = HttpContext.Session.GetString("username");
            //if (user != null)
            //{
            //return RedirectToAction("UserProfile");
            //}
            ViewBag.InvalidCode = HttpContext.Session.GetString("noticecode");
            HttpContext.Session.Remove("noticecode");
            ViewBag.InvalidCode2 = HttpContext.Session.GetString("noticeincode");
            HttpContext.Session.Remove("noticeincode");
            ViewBag.InvalidCode1 = "để gửi lại mã xác nhận.";
            ViewBag.IsConfirmed = HttpContext.Session.GetString("noticeisconfirm");
            HttpContext.Session.Remove("noticeisconfirm");
            ViewBag.ReSend = HttpContext.Session.GetString("noticeReSend");
            HttpContext.Session.Remove("noticeReSend");
            return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPasswordAsync(string username)
		{
			var userId = username;
			var code = GenerateVerificationCode();
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _cache.Set(userId, code, TimeSpan.FromMinutes(10));
            

            //var code = "qwert";
            //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            userId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(userId));
			var content = Url.Action("ChangePassword", "Account", new { userId = userId, code = code }, protocol: Request.Scheme);
			var check = _db.TblUsers.FirstOrDefault(p => p.Username == username);

			if (check != null)
			{
				await EmailSender.SendEmailAsync(username, "Quên mật khẩu", "<a href=\"" + content + "\" class=\"linkdetail\" style=\"text-decoration: none; margin: 0 auto; color: black;\">Thay đổi mật khẩu</a>");
                HttpContext.Session.SetString("email", username);

                ViewBag.ConfirmForgotSuccess = "*Vui lòng kiểm tra ";
				return View();
			}
			else
			{
				ViewBag.ErrorConfirmForogot = "*Email chưa được đăng ký.";
				return View();
			}
		}

        public async Task<IActionResult> ReSendEmailForgot()
        {
            var email = HttpContext.Session.GetString("email");
            if (email != null)
            {
                var code = GenerateVerificationCode();
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _cache.Set(email, code, TimeSpan.FromMinutes(10));
                var email1 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(email));

                var content = Url.Action("ChangePassword", "Account", new { userId = email1, code = code }, protocol: Request.Scheme);

                await EmailSender.SendEmailAsync(email, "Quên mật khẩu", "<a href=\"" + content + "\" class=\"linkdetail\" style=\"text-decoration: none; margin: 0 auto; color: black;\">Thay đổi mật khẩu</a>");
                string notice = "Hệ thống đã gửi lại mã xác nhận, vui lòng kiểm tra ";
                HttpContext.Session.SetString("noticeReSend", notice);

            }
            return RedirectToAction("ForgotPassword");

        }



        public IActionResult ChangePassword(string userId, string code)
		{
            //var user = HttpContext.Session.GetString("username");
            //if (user != null)
            //{
            //return RedirectToAction("UserProfile");
            //}
            userId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userId));
            var user = HttpContext.Session.GetString("username");
            string cachedVerificationCode = _cache.Get<string>(userId);
            if (userId != null && code == cachedVerificationCode)
            {
                var check = _db.TblUsers.FirstOrDefault(p => p.Username == userId);
                if (check != null)
                {
                    ViewBag.User = userId;
                    return View();
                }
                
			}
            else if (userId != null && cachedVerificationCode == null)
            {
                //var check = _db.TblUsers.FirstOrDefault(p => p.Username == userId && p.IsConfirm == true);
                //if (check != null)
                //{
                //    string isConfirmed = "Tài khoản của bạn đã được xác thực trước đó.";
                //    HttpContext.Session.SetString("noticeisconfirm", isConfirmed);
                //}
                //else
                //{
                    string invalidCode = "Mã xác nhận đã hết thời gian hiệu lực, nhấn ";
                    HttpContext.Session.SetString("noticecode", invalidCode);
                //}               
            }
            else
            {
                string invalid = "Mã xác nhận không tồn tại.";
                HttpContext.Session.SetString("noticeincode", invalid);
            }

            return RedirectToAction("ForgotPassword");
        }

		[HttpPost]
		public IActionResult ChangePassword(TblUser user)
		{
			string username = user.Username;
			string newPassword = user.Password;

			var check = _db.TblUsers.FirstOrDefault(p => p.Username == username);
			if (check != null)
			{
				check.Password = newPassword;
				_db.SaveChanges();
				string changeSucces = "Thay đổi mật khẩu thành công";
			}
			return RedirectToAction("Login");
		}

		public IActionResult UserProfile()
		{
			var user = HttpContext.Session.GetString("username");
			if (user != null)
			{
				ViewBag.ID = HttpContext.Session.GetString("id");
				var profile = _db.TblUsers.FirstOrDefault(p => p.Username.Equals(user));
				string fullname = profile.Firstname + " " + profile.Lastname;
				HttpContext.Session.SetString("fullname", fullname);
				ViewBag.Fullname = HttpContext.Session.GetString("fullname");
				return View(profile);
			}
			return RedirectToAction("Login");

		}

		public IActionResult EditUserProfile()
		{
			var user = HttpContext.Session.GetString("username");
			ViewBag.ID = HttpContext.Session.GetString("id");
			var profile = _db.TblUsers.FirstOrDefault(p => p.Username.Equals(user));
			ViewBag.Fullname = HttpContext.Session.GetString("fullname");
			if (user == null)
			{
				return RedirectToAction("Login");
			}
			return View(profile);
		}

		[HttpPost]
		public IActionResult EditUserProfile(string firstname, string lastname, string numberPhone)
		{
			var user = HttpContext.Session.GetString("username");
			ViewBag.ID = HttpContext.Session.GetString("id");
			ViewBag.Fullname = HttpContext.Session.GetString("fullname");
			if (user == null)
			{
				return RedirectToAction("Login");
			}
			else
			{
				var profile = _db.TblUsers.FirstOrDefault(p => p.Username.Equals(user));
				profile.Firstname = firstname;
				profile.Lastname = lastname;
				profile.Numberphone = numberPhone;
				_db.SaveChanges();
				return RedirectToAction("UserProfile");
			}

		}

		public IActionResult UserLocation()
		{
			ViewBag.ID = HttpContext.Session.GetString("id");
			ViewBag.Fullname = HttpContext.Session.GetString("fullname");
			var user = HttpContext.Session.GetString("username");
			var userid = _db.TblUsers.Where(p => p.Username.Equals(user)).Select(p => p.UserId).First();

			ViewBag.Phone = _db.TblUsers.Where(p => p.Username.Equals(user)).Select(p => p.Numberphone).First();
			ViewBag.UserID = userid.ToString();
			if (user == null)
			{
				return RedirectToAction("Login");
			}
			else
			{
				var addressDefault = _db.TblAddresses.FirstOrDefault(p => p.UserId == userid && p.IsDefault == true);
				if (addressDefault != null)
				{
					int addressID = _db.TblAddresses.Where(p => p.UserId == userid && p.IsDefault == true).Select(p => p.AddressId).First();
					ViewBag.AddressID = addressID;
					ViewBag.DefaultAdd = addressDefault.Address;
					ViewBag.DefaultBlock = addressDefault.BlockVillage;
					ViewBag.DefaultDis = addressDefault.District;
					ViewBag.DefaultCity = addressDefault.CityProvince;
					ViewBag.Checkbox = addressDefault.IsDefault;
				}
				var addList = _db.TblAddresses.Where(p => p.UserId == userid && p.IsDefault == false).ToList();
				return View(addList);
			}

		}

		[HttpPost]
		public IActionResult AddLocation(TblAddress model)
		{
			var user = HttpContext.Session.GetString("username");
			ViewBag.ID = HttpContext.Session.GetString("id");
			ViewBag.User = user;
			ViewBag.Fullname = HttpContext.Session.GetString("fullname");
			if (user == null)
			{
				return RedirectToAction("Login");
			}
			else
			{
				if (model.IsDefault == true)
				{
					var check = _db.TblAddresses.Where(p => p.UserId == model.UserId).ToList();
					foreach (var item in check)
					{
						item.IsDefault = false;
						_db.SaveChanges();
					}
					_db.Add(model);
					_db.SaveChanges();
					return RedirectToAction("UserLocation");
				}
				else
				{
					_db.Add(model);
					_db.SaveChanges();
					return RedirectToAction("UserLocation");
				}

			}

		}

		[HttpPost]
		public IActionResult RemoveLocation(int addressID)
		{
			var check = _db.TblAddresses.FirstOrDefault(p => p.AddressId == addressID);
			_db.Remove(check);
			_db.SaveChanges();
			return RedirectToAction("UserLocation");
		}

		[HttpPost]
		public IActionResult EditLocation(TblAddress model)
		{
			var check1 = _db.TblAddresses.FirstOrDefault(p => p.AddressId == model.AddressId);
			if (check1 != null)
			{
				if (model.IsDefault == true)
				{
					var check = _db.TblAddresses.Where(p => p.UserId == model.UserId).ToList();
					foreach (var item in check)
					{
						item.IsDefault = false;
						_db.SaveChanges();
					}
					check1.Address = model.Address;
					check1.BlockVillage = model.BlockVillage;
					check1.District = model.District;
					check1.CityProvince = model.CityProvince;
					check1.IsDefault = model.IsDefault;
					_db.SaveChanges();
					return RedirectToAction("UserLocation");
				}
				else
				{
					check1.Address = model.Address;
					check1.BlockVillage = model.BlockVillage;
					check1.District = model.District;
					check1.CityProvince = model.CityProvince;
					check1.IsDefault = model.IsDefault;
					_db.SaveChanges();
					return RedirectToAction("UserLocation");
				}
			}
			return RedirectToAction("UserLocation");
		}

		public IActionResult UserChangePassword()
		{
			var user = HttpContext.Session.GetString("username");
			ViewBag.ID = HttpContext.Session.GetString("id");
			ViewBag.Fullname = HttpContext.Session.GetString("fullname");
			ViewBag.User = user;
			if (user == null)
			{
				return RedirectToAction("Login");
			}
			return View();
		}

		[HttpPost]
		public IActionResult UserChangePassword(string username, string oldPassword, string password)
		{
			var user = HttpContext.Session.GetString("username");
			ViewBag.ID = HttpContext.Session.GetString("id");
			ViewBag.Fullname = HttpContext.Session.GetString("fullname");
			ViewBag.User = user;
			var check = _db.TblUsers.FirstOrDefault(p => p.Username == username && p.Password == oldPassword);
			if (check != null)
			{
				check.Password = password;
				_db.SaveChanges();
				ViewBag.ChangeSuccess = "*Thay đổi mật khẩu thành công.";
				return View();
			}
			ViewBag.ChangeFail = "*Mật khẩu hiện tại không chính xác, vui lòng thử lại.";
			return View();
		}

		public IActionResult UserHistoryOrder(int? page)
		{
			ViewBag.ID = HttpContext.Session.GetString("id");
			ViewBag.Fullname = HttpContext.Session.GetString("fullname");
			var user = HttpContext.Session.GetString("username");
			var userID = _db.TblUsers.Where(p => p.Username.Equals(user)).Select(p => p.UserId).FirstOrDefault();
			ViewBag.User = user;
			if (user == null)
			{
				return RedirectToAction("Login");
			}
			else
			{
				var orderList = _db.TblOrders.Where(p => p.UserId == userID).ToList();
				var resultOrderList = orderList.OrderByDescending(x => x.Date).ToList();
                int pageSize = 4;
				int pageNumber = (page ?? 1);
				return View(resultOrderList.ToPagedList(pageNumber, pageSize));
			}
		}

		[HttpPost]
		public IActionResult OrderDetail(string orderID)
		{
			ViewBag.ID = HttpContext.Session.GetString("id");
			ViewBag.Fullname = HttpContext.Session.GetString("fullname");
			var user = HttpContext.Session.GetString("username");
			ViewBag.User = user;
			if (user == null)
			{
				return RedirectToAction("Login");
			}
			else
			{

				var orderListDetail = _db.TblOrderDetails.Where(p => p.OrderId.Equals(orderID));

				var result = from _item in orderListDetail
							 from product in _db.TblProducts
							 from img in _db.TblImages
							 where _item.ProductId.Equals(product.ProductId) && product.ProductId.Equals(img.RelationId)
							 select new OrderDetailInfoModel
							 {
								 productName = product.Name,
								 image = img.Url,
								 Quantity = _item.Quantity,
								 Price = _item.Price,
							 };

				var date = _db.TblOrderDetails.Where(p => p.OrderId == orderID).Select(p => p.Date).FirstOrDefault();
				var total = _db.TblOrders.Where(p => p.OrderId == orderID).Select(p => p.TotalPrice).FirstOrDefault();

				var phone = _db.TblOrders.Where(p => p.OrderId == orderID).Select(p => p.Phone).FirstOrDefault();
				ViewBag.Phone = phone;
				var status = _db.TblOrders.Where(p => p.OrderId == orderID && p.Status == 2).FirstOrDefault();
				ViewBag.status = status;	
				var address = _db.TblOrders.Where(p => p.OrderId == orderID).Select(p => p.Address).FirstOrDefault();
				ViewBag.Address = address;

				ViewBag.OrderID = orderID;
				ViewBag.Date = date;
				ViewBag.TotalPrice = total.ToString();
				ViewBag.Email = user;


				return View(result.ToList());
			}

		}

		public IActionResult ReceiveButton(string returnUrl = "/")
		{
			var properties = new AuthenticationProperties { RedirectUri = returnUrl };
			return Challenge(properties, "Google");
			//return new ChallengeResult("Google", null);
		}

		public IActionResult LoginWithGoogle(string code)
		{
			//var userInfo = await _googleAuthService.GetUserInfoAsync(code);
			//if (userInfo != null)
			//{

			//    //_httpContextAccessor.HttpContext.Session.SetString("GoogleUserId", userInfo.Id);
			//    //_httpContextAccessor.HttpContext.Session.SetString("GoogleUserName", userInfo.UserName);
			//    //_httpContextAccessor.HttpContext.Session.SetString("GoogleUserEmail", userInfo.Email);
			//    HttpContext.Session.SetString("username", userInfo.Email);
			//}
			HttpContext.Session.SetString("username", code);
			return RedirectToAction("Index", "Home");
		}
		
		public IActionResult Refund()
		{
			var user = HttpContext.Session.GetString("username");
			var userid = _db.TblUsers.Where(p => p.Username.Equals(user)).Select(p => p.UserId).First();
			ViewBag.userId = userid;
			if (user == null)
			{
				return RedirectToAction("Login");
			}
			else
			{

				var refundd = from r in _db.TblRefunds
							  join u in _db.TblUsers on r.UserId equals u.UserId
							  select new RefundForm()
							  {
								  refundId = r.RefundId,
								  userId = u.UserId,
								  content = r.Description,
								  status = r.IsConfirm,
								  orderId = r.OrderId,
								  date = r.Date
							  };
				
                return View(refundd.Where(p=>p.userId == userid));
			}

		}
		[HttpPost]
		public async Task<IActionResult> AddRefundAsync(string orderid, string note, List<IFormFile> files)
		{
			string url = "";
			var user = HttpContext.Session.GetString("username");
			var userid = _db.TblUsers.Where(p => p.Username.Equals(user)).Select(p => p.UserId).First();
				var check = _db.TblRefunds.Where(p=>p.OrderId == orderid).Select(p => p.OrderId).FirstOrDefault();

			if (orderid.Equals(check))
			{
				return Json(new
				{
					messge = "Bạn đã yêu cầu hoàn trã này trong hệ thống",
				});
			}
			else
			{
                TblRefund refund = new TblRefund()
                {
                    OrderId = orderid,
                    RefundId = Guid.NewGuid().ToString(),
                    Description = note,
                    Date = DateTime.Now,
                    UserId = userid,
                    IsConfirm = null,
                };

				var tblOrder = _db.TblOrders.Where(p => p.OrderId == orderid).FirstOrDefault();
				if(tblOrder != null)
				{
					tblOrder.Status = 4;
					_db.Update(tblOrder);

                }
                _db.TblRefunds.Add(refund);
				_db.TblRefunds.Add(refund);
				url = await FirebaseService.UploadImage(files, "products");
				TblImage tblImage = new TblImage
				{
					ImageId = Guid.NewGuid().ToString(),
					Name = "Refund img",
					RelationId = refund.OrderId,
					Type = "Product",
					Url = url
				};
				_db.TblImages.Add(tblImage);
				_db.SaveChanges();
				return RedirectToAction("Refund", "Account");
			}
               
			
		}

	}
}