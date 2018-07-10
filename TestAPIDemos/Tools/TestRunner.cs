using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Remote;
using System;

namespace TestAPIDemos.Tools
{
    [TestFixture]
    public abstract class TestRunner
    {
        private readonly int IMPLICIT_WAIT = 30;
        private string uriUnderTest = "http://127.0.0.1:4723/wd/hub";

        protected AppiumDriver<AndroidElement> driver;
        private DesiredCapabilities capabilities;
        protected int index;

        protected int startX;
        protected int startY;
        protected int endX;
        protected int endY;
        
        
        /// <summary>
        /// Before all test methods
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            capabilities = new DesiredCapabilities();

            capabilities.SetCapability(MobileCapabilityType.App, @"D:\Android\demo3\ApiDemos-debug.apk");
            capabilities.SetCapability(MobileCapabilityType.DeviceName, "5ba6cc09");//emulator-5554
            capabilities.SetCapability(MobileCapabilityType.Udid, "5ba6cc09");
            capabilities.SetCapability(MobileCapabilityType.PlatformVersion, "6.0.0");
            capabilities.SetCapability(MobileCapabilityType.PlatformName, "Android");
            capabilities.SetCapability(MobileCapabilityType.FullReset, "false");
        }

        /// <summary>
        /// Before each tests
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Questions!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            driver = new AndroidDriver<AndroidElement>(new Uri(uriUnderTest), capabilities);

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(IMPLICIT_WAIT));

            index = 0;
        }

        /// <summary>
        /// Affter all tests methods close app
        /// </summary>
        [OneTimeTearDown]
        public void AfterAllMethods()
        {
            driver.CloseApp();
            driver.Quit();
        }

        public void SwipeTo(string way)
        {
            var size = driver.Manage().Window.Size;

            startY = (int)(size.Height * 0.75);//0.85 --all page//0.80
            endY = (int)(size.Height * 0.25);//0.15 ---all page//0.20
            startX = (size.Width / 2);
            if (way == "down")
            {
                driver.Swipe(startX, startY, startX, endY, 1000);
            }
            else if (way == "up")
            {
                driver.Swipe(startX, endY, startX, startY, 1000);
            }
        }
    }
}
