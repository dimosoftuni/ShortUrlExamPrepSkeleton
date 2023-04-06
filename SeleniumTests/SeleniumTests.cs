using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    public class SeleniumTests
    {

        private WebDriver driver;
        private const string baseUrl = "https://shorturl.dimomitev.repl.co/";
        // private const string baseUrl = "http://localhost:8080";

        [SetUp]
        public void OpenWebApp()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Url = baseUrl;
        }

        [TearDown]
        public void CloseWebApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_VerifyTableTopLeftCell()
        {
            // Navihate to ShortURL page
            var linkShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            linkShortUrl.Click();

            var tableHeaderLeftCell = driver.FindElement(By.CssSelector("th:nth-child(1)"));

            Assert.That(tableHeaderLeftCell.Text, Is.EqualTo("Original URL"), "Upper left cell text");
        }


        [Test]
        public void Test_AddValidUrl()
        {
            var urlToAdd = "http://url" + DateTime.Now.Ticks + ".com";

            var linkAddUrl = driver.FindElement(By.LinkText("Add URL"));
            linkAddUrl.Click();

            var inputAddUrl = driver.FindElement(By.Id("url"));
            inputAddUrl.SendKeys(urlToAdd);

            var buttonCreate = driver.FindElement(By.XPath("//button[@type='submit']"));
            buttonCreate.Click();

            // Assertion for the URL in the Page Source
            Assert.That(driver.PageSource.Contains(urlToAdd));

            var tableLastRow = driver.FindElements(By.CssSelector("table > tbody > tr")).Last();
            var tableLastRowFirstCell = tableLastRow.FindElements(By.CssSelector("td")).First();

            Assert.That(tableLastRowFirstCell.Text, Is.EqualTo(urlToAdd), "URL text");
        }

        [Test]
        public void Test_AddInvalidUrl()
        {
            var linkAddUrl = driver.FindElement(By.LinkText("Add URL"));
            linkAddUrl.Click();

            var inputAddUrl = driver.FindElement(By.Id("url"));
            inputAddUrl.SendKeys("alabla");

            var buttonCreate = driver.FindElement(By.XPath("//button[@type='submit']"));
            buttonCreate.Click();

            var labelErrorMessage = driver.FindElement(By.XPath("//div[@class='err']"));
            Assert.That(labelErrorMessage.Text, Is.EqualTo("Invalid URL!"));

            Assert.True(labelErrorMessage.Displayed, "Error message is not displayed");
        }

        [Test]
        public void Test_VisitNonExistingUrl()
        {
            // driver.Navigate().GoToUrl("http://shorturl.nakov.repl.co/go/invalid536524");
            driver.Url = "http://shorturl.nakov.repl.co/go/invalid536524";
            
            var labelErrorMessage = driver.FindElement(By.XPath("//div[@class='err']"));
            Assert.That(labelErrorMessage.Text, Is.EqualTo("Cannot navigate to given short URL"));

            Assert.True(labelErrorMessage.Displayed, "Error message is not displayed");
        }

        [Test]
        public void Test_VerifyCounterIncrease()
        {
            // Navihate to ShortURL page
            var linkShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            linkShortUrl.Click();

            var tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var oldCounter = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            var linkToClickCell = tableFirstRow.FindElements(By.CssSelector("td"))[1];

            var linkToClick = linkToClickCell.FindElement(By.TagName("a"));
            linkToClick.Click();

            driver.SwitchTo().Window(driver.WindowHandles[0]);

            driver.Navigate().Refresh();

            tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var newCounter = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            Assert.That(newCounter, Is.EqualTo(oldCounter + 1));
        }
    }
}