using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using Xunit;

namespace EHU.WebUITests.xUnit
{
    public class EHUWebsiteTests : IDisposable
    {
        private IWebDriver driver;

        public EHUWebsiteTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public void Dispose()
        {
            driver.Quit();
        }

        [Fact(DisplayName = "About EHU page navigation"), Trait("Category", "Smoke")]
        public void VerifyAboutEHUPageNavigation()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var aboutLink = driver.FindElement(By.XPath("//a[contains(text(),'About EHU')]"));
            aboutLink.Click();

            Assert.Equal("https://en.ehu.lt/about/", driver.Url);
            Assert.Equal("About EHU", driver.Title);

            var contentHeader = driver.FindElement(By.XPath("//h1[contains(text(),'About European Humanities University')]"));
            Assert.True(contentHeader.Displayed);
        }

        [Theory(DisplayName = "Search functionality test"), Trait("Category", "Regression")]
        [InlineData("study programs")]
        [InlineData("philosophy")]
        public void VerifySearchFunctionality(string query)
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var searchBox = driver.FindElement(By.CssSelector("input[name='s']"));
            searchBox.SendKeys(query);
            searchBox.SendKeys(Keys.Enter);

            Assert.Contains($"/?s={query.Replace(" ", "+")}", driver.Url);

            var searchResults = driver.FindElements(By.CssSelector("article"));
            Assert.True(searchResults.Count > 0);
        }

        [Fact(DisplayName = "Change language to Lithuanian"), Trait("Category", "Smoke")]
        public void VerifyLanguageChangeToLithuanian()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var languageSwitcher = driver.FindElement(By.CssSelector(".lang-item-lt a"));
            languageSwitcher.Click();

            Assert.Equal("https://lt.ehu.lt/", driver.Url);

            var lithuanianText = driver.FindElement(By.XPath("//*[contains(text(),'Europos humanitarinis universitetas')]"));
            Assert.True(lithuanianText.Displayed);
        }

        [Fact(DisplayName = "Contact info verification"), Trait("Category", "Regression")]
        public void VerifyContactInformation()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");

            var email = driver.FindElement(By.XPath("//a[contains(@href,'mailto:franciskscarynacr@gmail.com')]"));
            Assert.Equal("franciskscarynacr@gmail.com", email.Text.Trim());

            var phoneLT = driver.FindElement(By.XPath("//a[contains(@href,'tel:+37068771365')]"));
            Assert.Equal("+370 68 771365", phoneLT.Text.Trim());

            var phoneBY = driver.FindElement(By.XPath("//a[contains(@href,'tel:+375295781488')]"));
            Assert.Equal("+375 29 5781488", phoneBY.Text.Trim());

            Assert.True(driver.FindElement(By.XPath("//a[contains(@href,'facebook.com')]")) != null);
            Assert.True(driver.FindElement(By.XPath("//a[contains(@href,'t.me')]")) != null);
            Assert.True(driver.FindElement(By.XPath("//a[contains(@href,'vk.com')]")) != null);
        }
    }

    [CollectionDefinition("Parallel EHU Tests", DisableParallelization = false)]
    public class ParallelTestsCollection : ICollectionFixture<EHUWebsiteTests> { }
}
