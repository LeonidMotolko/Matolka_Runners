using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace EHU.WebUITests
{
    [TestFixture]
    public class EHUWebsiteTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

        [Test]
        public void VerifyAboutEHUPageNavigation()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            Thread.Sleep(2000);

            var aboutLink = driver.FindElement(By.XPath("//a[contains(text(),'About EHU')]"));
            aboutLink.Click();

            Assert.AreEqual("https://en.ehu.lt/about/", driver.Url);
            Assert.AreEqual("About EHU", driver.Title);

            var contentHeader = driver.FindElement(By.XPath("//h1[contains(text(),'About European Humanities University')]"));
            Assert.IsTrue(contentHeader.Displayed);
        }

        [Test]
        public void VerifySearchFunctionality()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            Thread.Sleep(2000);

            var searchBox = driver.FindElement(By.CssSelector("input[name='s']"));
            searchBox.SendKeys("study programs");
            searchBox.SendKeys(Keys.Enter);

            Thread.Sleep(2000);
            Assert.That(driver.Url, Does.Contain("/?s=study+programs"));

            var searchResults = driver.FindElements(By.CssSelector("article"));
            Assert.Greater(searchResults.Count, 0);
        }

        [Test]
        public void VerifyLanguageChangeToLithuanian()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            Thread.Sleep(2000);

            var languageSwitcher = driver.FindElement(By.CssSelector(".lang-item-lt a"));
            languageSwitcher.Click();

            Thread.Sleep(2000);
            Assert.AreEqual("https://lt.ehu.lt/", driver.Url);

            var lithuanianText = driver.FindElement(By.XPath("//*[contains(text(),'Europos humanitarinis universitetas')]"));
            Assert.IsTrue(lithuanianText.Displayed);
        }

        [Test]
        public void VerifyContactInformation()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");
            Thread.Sleep(2000);

            var email = driver.FindElement(By.XPath("//a[contains(@href,'mailto:franciskscarynacr@gmail.com')]"));
            Assert.AreEqual("franciskscarynacr@gmail.com", email.Text.Trim());
            
            var phoneLT = driver.FindElement(By.XPath("//a[contains(@href,'tel:+37068771365')]"));
            Assert.AreEqual("+370 68 771365", phoneLT.Text.Trim());

            var phoneBY = driver.FindElement(By.XPath("//a[contains(@href,'tel:+375295781488')]"));
            Assert.AreEqual("+375 29 5781488", phoneBY.Text.Trim());

            Assert.IsTrue(driver.FindElement(By.XPath("//a[contains(@href,'facebook.com')]")).Displayed);
            Assert.IsTrue(driver.FindElement(By.XPath("//a[contains(@href,'t.me')]")).Displayed);
            Assert.IsTrue(driver.FindElement(By.XPath("//a[contains(@href,'vk.com')]")).Displayed);
        }
    }
}
