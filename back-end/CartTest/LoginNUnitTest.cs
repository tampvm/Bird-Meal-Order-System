using BMOS.Controllers;
using BMOS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMOSTest
{
	//public class LoginNUnitTest
	//{
	//	private AccountController controller;
	//	[SetUp]
	//	public void Setup()
	//	{
	//		controller = new AccountController();
	//	}

	//	[Test]
	//	public void TestLoginWithCorrectUsernameAndPassword()
	//	{
	//		var result = (RedirectToActionResult)controller.Login("kenandkq@gmail.com", "12345");
	//		Assert.AreEqual("Index", result.ActionName);
	//	}

	//	[Test]
	//	public void TestLoginWithCorrectUsernameAndIncorrectPassword()
	//	{
	//		var result = controller.Login("kenandkq@gmail.com", "12345") as ViewResult;
	//		Assert.AreEqual("Login", result.ViewName);

	//	}

	//	[Test]
	//	public void TestLoginWithInvalidUsername()
	//	{
	//		var result = controller.Login("wrongemail@gmail.com", "123") as ViewResult;
	//		Assert.AreEqual("Login", result.ViewName);
	//	}

	//	[Test]
	//	public void TestLoginWithEmpty()
	//	{
	//		var result = controller.Login("", "") as ViewResult;
	//		Assert.AreEqual("Login", result.ViewName);
	//	}
	//}
}
