using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MimeKit.Text;
using BMOS.Helpers;
using BMOS.Models;
using BMOS.Models.Entities;
using System.Drawing;
using Microsoft.CodeAnalysis.CSharp;

namespace BMOS.Services
{
	public class EmailSender
	{
		public static Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var emailToSend = new MimeMessage();
			//emailToSend.Sender = new MailboxAddress("Bird Meal Order System", "gokutam123@gmail.com");
			//emailToSend.From.Add(MailboxAddress.Parse("gokutam123@gmail.com"));
			emailToSend.From.Add(new MailboxAddress("Bird Meal Order System", "minhtam250102@gmail.com"));
			emailToSend.To.Add(MailboxAddress.Parse(email));
			emailToSend.Subject = subject;

			string htmlBody = @"
            <!DOCTYPE html>
            <html lang=""en"">

            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            </head>

            <body>
                <div id=""wrapper"" style=""display: flex; align-items: center; justify-content: center; margin: 0px auto; max-width:600px;"">
                    <div class=""form"" style=""width: 600px; height: 450px; border: 1px solid black; border-radius: 10px;"">
                        <div class=""header"" style=""border: 1px solid gray; border-radius: 10px; background-color: #EFC15F;"">
                            <div class=""logo"" style=""height: 90px; width: 200px; background-color: white; margin: 10px auto; border-radius: 10px; display: flex; align-items: center; justify-content: center;"">
                                <img src=""https://firebasestorage.googleapis.com/v0/b/bmosfile.appspot.com/o/Logo.png?alt=media&token=bec2eac4-0dea-4f53-b881-4dda53481ae5"" alt=""Logo"" srcset="""" style=""width: 100px; height: 100%; margin: 0 auto;"">
                            </div>
                            <div class=""text"" style=""text-align: center;"">
                                <h1>Chào mừng bạn đến với BMOS</h1>
                                <div class=""link"" style=""width: 150px; height: 30px; background-color: aliceblue; border-radius: 5px; margin: 0 auto; display: flex; align-items: center; justify-content: center; border: 1px solid black;"">"
                                + htmlMessage +
								@"</div>
                                <div class=""headertext"" style=""margin-top: 5px; margin-bottom: 10px;"">
                                    <span>(Chúng tôi cần xác thực địa chỉ email của bạn để kích hoạt tài khoản)</span>
                                </div>
                            </div>
                        </div>

                        <div class=""footer"" style=""display: flex; align-items: center; justify-content: center;"">
                            <div class=""footercontent"" style=""width: 480px; text-align: center; margin: 0px auto;"">
                                <div class=""span1"" style=""margin-top: 10px; margin-bottom: 10px;"">
                                    <span>Copyright © 2023 BMOS, Allrights reserved.</span>
                                </div>
                                <div><b>Địa chỉ của chúng tôi:</b></div>
                                <div class=""span2"" style=""margin-top: 10px;"">
                                    <span>C11-01, số 473 Man Thiện, Thủ Đức, KDT Geleximco Lê Trọng Tấn, Phường Dương Nội, Quận Hà Đông, Hà Nội</span></div>
                                <div class=""span3"" style=""margin-top: 10px;"">
                                    <span>Email: group7se@gmail.com</span>
                                </div>
                                <div class=""span4"" style=""margin-top: 10px;""><span>Hotline: 0979500611</span></div>
                            </div>
                         </div>
                      </div>
                   </div>
               </body>
            </html>";

			emailToSend.Body = new TextPart(TextFormat.Html)
			{
				Text = htmlBody
			};

			using (var emailClient = new SmtpClient())
			{
				emailClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
				emailClient.Authenticate("minhtam250102@gmail.com", "ihrhvyplgkonevqy");
				emailClient.Send(emailToSend);
				emailClient.Disconnect(true);
			}
			return Task.CompletedTask;
		}

		public static Task SendEmailConfirmOrderAsync(string email, string subject, string htmlMessage, int cartQuantity, double? cartPrice, TblOrder order, string fullName)
		{
			var emailToSend = new MimeMessage();
			//emailToSend.Sender = new MailboxAddress("Bird Meal Order System", "gokutam123@gmail.com");
			//emailToSend.From.Add(MailboxAddress.Parse("gokutam123@gmail.com"));
			emailToSend.From.Add(new MailboxAddress("Bird Meal Order System", "minhtam250102@gmail.com"));
			emailToSend.To.Add(MailboxAddress.Parse(email));
			emailToSend.Subject = subject;
            var price = string.Format("{0:#,0}", cartPrice);
			string htmlBody = @"
            <!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Document</title>
    <style>
        .email {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
  font-family: Arial, sans-serif;
  background-color: #f9f9f9;
}

h1 {
  font-size: 24px;
  margin-top: 0;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 20px;
}

th {
  text-align: left;
  padding: 5px;
  border-bottom: 1px solid #ccc;
}

td {
  padding: 5px;
  border-bottom: 1px solid #ccc;
}

table:last-of-type {
  margin-bottom: 0;
}

p:last-of-type {
  margin-bottom: 0;
}
    </style>
</head>
<body>
    <div class=""email"">
        <h1>Cảm ơn sản bạn đã mua hàng!</h1>
        <p>Dear ["+ fullName + @"],</p>
        <p>Chúng tôi vừa xác nhận bạn có đơn hàng trên [BMOS]. Chúng tôi thực sự cảm ơn bạn và mong bạn xác nhận!</p>
        <div style=""margin: 0 auto; width: 100%; height: 100px; text-align: center;"">
            <img src=""https://firebasestorage.googleapis.com/v0/b/bmosfile.appspot.com/o/Logo.png?alt=media&token=bec2eac4-0dea-4f53-b881-4dda53481ae5"" alt=""Logo"" srcset="""" style=""width: 100px; height: 100%;"">
        </div>
        <p>Chi tiết đơn hàng của bạn</p>
        <table>
          <tr>
            <td>Mã order: </td>
            <td>"+ order.OrderId +  @"</td>
          </tr>
          <tr>
            <td>Ngày mua:</td>
            <td>"+order.Date.ToString()+@"</td>
          </tr>
          <tr>
            <td>Địa chỉ giao:</td>
            <td>"+order.Address+ @"</td>
          </tr>
          <tr>
            <td>Số điện thoại:</td>
            <td>"+order.Phone+@"</td>
          </tr>
        </table>
        <p>Items:</p>
        <table>
          <tr>
            <th>Số lượng sản phẩm</th>
            <th></th>
            <th>Tổng tiền</th>
          </tr>
          <tr>
            <td style=""width: 300px; "">["+cartQuantity+ @" sản phẩm]</td>
            <td></td>
            <td>["+ price + @"vnđ]</td>
          </tr>
        </table>
        <p>Đơn hàng của bạn sẽ được tích điểm sau <b>5 ngày</b>, <i><b>nếu trong vòng 5 ngày tới bạn hoàn trả sản phẩm, thì điểm này sẽ không được tích.</b></i></p>
        <p>Cảm ơn sự lựa chọn của bạn dành cho [<b>>BMOS</b>]!</p>        
        <p>Bạn có thể xem lại lịch sử đơn hàng của bạn trong phần [<b>Trang cá nhân</b>]!</p>
       
        <button style=""background-color: #efc15f; border: none; outline: none; padding: 10px; width: 100px; margin: 10px 0;"">" + htmlMessage + @"</button>
        <p>Best regards,</p>
      </div>
</body>
</html>
         

";

			emailToSend.Body = new TextPart(TextFormat.Html)
			{
				Text = htmlBody
			};

			using (var emailClient = new SmtpClient())
			{
				emailClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
				emailClient.Authenticate("minhtam250102@gmail.com", "ihrhvyplgkonevqy");
				emailClient.Send(emailToSend);
				emailClient.Disconnect(true);
			}
			return Task.CompletedTask;
		}
	}
}
