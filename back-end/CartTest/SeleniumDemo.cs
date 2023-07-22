using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;

namespace BMOS
{
	class SeleniumDemo
	{
		IWebDriver driver;

		[SetUp]
		public void SetupTest()
		{
			driver = new ChromeDriver("D:\\TESTER\\SeleniumC#");
			driver.Manage().Window.Maximize();
		}

		[Test]
		public void AddSingleProductToCart()
		{
			driver.Navigate().GoToUrl("https://localhost:44388/Account/Login");

			Thread.Sleep(1000);
		
			Thread.Sleep(2000);
			IWebElement userName = driver.FindElement(By.Name("username"));
			userName.Clear();
			userName.SendKeys("123@gmail.com");
			Thread.Sleep(1000);
			IWebElement password = driver.FindElement(By.Name("password"));
			password.Clear();
			password.SendKeys("123");

			driver.FindElement(By.Id("submit")).Click();

			Thread.Sleep(1000);

			driver.Navigate().GoToUrl("https://localhost:44388/products/product/254f5bd3-f681-42cc-a0f6-6468a8f143c6");

			IWebElement item = driver.FindElement(By.ClassName("button-action--add-cart"));
			item.Click();

			Thread.Sleep(2000);
			driver.Navigate().GoToUrl("https://localhost:44388/ShoppingCart");
			IWebElement itemQuantity = driver.FindElement(By.XPath("//h6[normalize-space()='1 items']"));

			string productInCart = itemQuantity.Text;
			Console.WriteLine(productInCart);
			Assert.AreEqual(productInCart, "1 items", "Tổng sản phẩm không đúng với sản phẩm add vào");
			Thread.Sleep(1000);
		}
		[Test]
		public void LoginWithWrongUserNameAndPassWord()
		{
			driver.Navigate().GoToUrl("https://localhost:44388/Account/Login");
			Thread.Sleep(2000);
			var usernameField = driver.FindElement(By.Id("customer_email"));
			var passwordField = driver.FindElement(By.Id("customer_password"));
			var loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));
			usernameField.SendKeys("invalid_username@example.com");
			passwordField.SendKeys("invalid_password");
			loginButton.Click();

			Thread.Sleep(2000);
			var errorMessage = driver.FindElement(By.ClassName("notice_text"));
			Assert.IsTrue(errorMessage.Displayed);

		}

		[TearDown]
		public void CloseTest()
		{
			driver.Close();
		}
	}
}