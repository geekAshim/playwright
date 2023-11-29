using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        IPage Page;

        [TestMethod]
        [TestInitialize]
        public async Task TestInitialize()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            Page = await browser.NewPageAsync();
        }

        [TestMethod]
        public async Task TestMethod1Async()
        {
            //Given I am on https://www.google.com
            await Page.GotoAsync("https://www.google.com");

            //When I type dotnetcoretutorials.com into the search box
            await Page.FillAsync("[title='Search']", "dotnetcoretutorials.com");

            //And I press the button with the text "Google Search"
            await Page.ClickAsync("[value='Google Search'] >> nth=1");

            //Then the first result is domain dotnetcoretutorials.com
            var firstResult = await Page.Locator("//cite >> nth=0").InnerTextAsync();
            Assert.AreEqual("https://dotnetcoretutorials.com", firstResult);
        }
    }
}
