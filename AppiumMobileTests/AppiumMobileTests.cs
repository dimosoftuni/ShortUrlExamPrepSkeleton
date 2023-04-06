using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace AppiumMobileTests
{
    public class AppiumMobileTests
    {
        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;

        private const string appLocation = @"C:\DemoApps\com.android.example.github.apk";
        private const string appiumServer = "http://127.0.0.1:4723/wd/hub";

        [SetUp]
        public void PrepareApp()
        {
            this.options = new AppiumOptions() { PlatformName = "Android" };
            options.AddAdditionalCapability("app", appLocation);
            driver = new AndroidDriver<AndroidElement>(new Uri(appiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_VerifyBarancevName()
        {
            var inputSearchField = driver.FindElementById("com.android.example.github:id/input");
            inputSearchField.Click();
            inputSearchField.SendKeys("Selenium\\n");

            // hit enter
            // driver.PressKeyCode(AndroidKeyCode.Enter);
            // driver.PressKeyCode(66);

            // Assert selenium text
            var textSelenium = driver.FindElementByXPath("//android.view.ViewGroup/android.widget.TextView[2]");
            Assert.That(textSelenium.Text, Is.EqualTo("SeleniumHQ/selenium"));

            // Click on Selenium result
            textSelenium.Click();

            // Assert Barancev is inside the list
            var listTextBarancev = driver.FindElementByXPath("//android.widget.FrameLayout[2]/android.view.ViewGroup/android.widget.TextView");
            Assert.That(listTextBarancev.Text, Is.EqualTo("barancev"));

            // Click on barancev text
            listTextBarancev.Click();

            // Assert Barancev name 
            var textFieldBarancev = driver.FindElementByXPath("//android.widget.TextView[@content-desc='user name']");
            Assert.That(textFieldBarancev.Text, Is.EqualTo("Alexei Barantsev"));
        }
    }
}