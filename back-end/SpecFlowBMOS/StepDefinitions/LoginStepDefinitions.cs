using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowBMOS.StepDefinitions
{
    [Binding]
    public class LoginStepDefinitions
    {
        IWebDriver driver = new ChromeDriver();

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            driver.Navigate().GoToUrl("https://localhost:44388/Account/Login");
            //driver.Navigate().GoToUrl("https://petfood.vn/account/login");
        }

        [When(@"I enter valid login credentials")]
        public void WhenIEnterValidLoginCredentials()
        {
            var usernameField = driver.FindElement(By.Id("customer_email"));
            var passwordField = driver.FindElement(By.Id("customer_password"));
            //var usernameField = driver.FindElement(By.Id("customer_email"));
            //var passwordField = driver.FindElement(By.Id("customer_password"));
            var loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));
            usernameField.SendKeys("gokutam123@gmail.com");
            passwordField.SendKeys("1");
        }

        [When(@"I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            var loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));
            loginButton.Click();
        }

        [Then(@"I should be redirected to the home page")]
        public void ThenIShouldBeRedirectedToTheHomePage()
        {
            Assert.AreEqual("https://localhost:44388/", driver.Url);
            //Assert.AreEqual("https://petfood.vn/account", driver.Url);
        }

        [When(@"I enter invalid login credentials")]
        public void WhenIEnterInvalidLoginCredentials()
        {
            var usernameField = driver.FindElement(By.Id("customer_email"));
            var passwordField = driver.FindElement(By.Id("customer_password"));
            var loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));
            usernameField.SendKeys("invalid_username@example.com");
            passwordField.SendKeys("invalid_password");
        }

        [Then(@"I should see an error message")]
        public void ThenIShouldSeeAnErrorMessage()
        {
            var errorMessage = driver.FindElement(By.ClassName("notice_text"));
            Assert.IsTrue(errorMessage.Displayed);
        }
    }
}
