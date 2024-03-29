using EPAM.PlaywrightFW.Common;
using EPAM.PlaywrightFW.Core;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Data.SqlTypes;

namespace EPAM.Playwright.Test
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class PlaywrightInjectableTest
    {
        private IBrowser? _driver;
        IPage Page;


        [OneTimeSetUp]
        public void Setup()
        {
            TestSession.Current.Start();
            _driver = Task.Run(() => TestSession.Current.Resolve<IBrowser>()).Result;
            var url = TestSession.Current.Settings.ApplicationUrl;
            Page = Task.Run(() => _driver.NewPageAsync()).Result;
        }

        [Test]
        public async Task Task1Test()
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

        [Test]
        public async Task Task3Test()
        {
            await Page.GotoAsync("https://www.google.com");
            string title = await Page.TitleAsync();
            string url = Page.Url;
            string pageSource = await Page.InnerHTMLAsync("*");
            Console.WriteLine(string.Concat("Title:", title));
            Console.WriteLine("Title Length:" + title.Length);
            Console.WriteLine(string.Concat("URL:", url));
            Console.WriteLine("URL Length:" + url.Length);
            Console.WriteLine(string.Concat("PageSource:", pageSource));
            Console.WriteLine("PageSource Length:" + pageSource.Length);
        }

        [Test]
        public void Task4Test()
        {
            Assert.IsTrue(Getpassword("abcdefgh"));
            Assert.IsTrue(Getpassword("abcdefghqwrt"));
            Assert.IsTrue(!Getpassword("abcdefg"));
            Assert.IsTrue(!Getpassword("abcdefghqwrtq"));
        }

        [Test]
        public async Task Task5Test()
        {
            await Page.GotoAsync("https://www.flipkart.com/");

            await Page.GetByRole(AriaRole.Button, new() { Name = "✕" }).ClickAsync();

            Assert.IsTrue(await Page.GetByLabel("Mobiles").IsVisibleAsync());
            Assert.IsTrue(await Page.GetByLabel("Fashion").IsVisibleAsync());
            Assert.IsTrue(await Page.GetByLabel("Electronics").IsVisibleAsync());
            //Assert.IsTrue(await Page.GetByLabel("Travel").IsVisibleAsync());
            Assert.IsTrue(await Page.GetByLabel("Appliances").IsVisibleAsync());
            //Assert.IsTrue(await Page.Locator("._1wE2Px").IsVisibleAsync());

            await Page.GetByLabel("Appliances").ClickAsync();
            await Page.GetByText("TVs & Appliances").ClickAsync();

            await Page.GetByRole(AriaRole.Link, new() { Name = "Split ACs" }).ClickAsync();

            ILocator acPrice = WaitUtil.WaitforAction<ILocator>(() =>
            {
                return Page.Locator(@"xpath=.//div[@class='_1AtVbE col-12-12']//div[@class='_30jeq3 _1_WHN1']").AllAsync().Result.FirstOrDefault();
            }, 30000);


            //ILocator acPrice = (await Page.Locator(@"xpath=.//div[@class='_1AtVbE col-12-12']//div[@class='_30jeq3 _1_WHN1']").AllAsync()).FirstOrDefault();
            string price = await acPrice.InnerTextAsync();
            //await acPrice.ClickAsync();

            var Page1 = await Page.RunAndWaitForPopupAsync(async () =>
            {
                //await Page.GetByRole(AriaRole.Link, new() { Name = "Panasonic Convertible 7-in-1 with Additional AI Mode Cooling 2023 Model 1.5 Ton 3 Star Split Inverter ... Add to Compare Sponsored Panasonic Convertible 7-in-1 with Additional AI Mode Cooling 2023 Model 1.5 Ton 3 Star Split Inverter ... 4.3 18,887 Ratings & 2,239 Reviews • Annual Power Usage: 1002.72 kWh • Room Size: 111 - 150 sqft • 1 Year Comprehensive Warranty on Product, 5 Years Warranty on PCB and 10 Years Warranty on Compressor ₹36,490 ₹55,400 34% off Free delivery Upto ₹6,000 Off on Exchange No Cost EMI from ₹4,055/month" }).ClickAsync();
                await acPrice.ClickAsync();
            });

            //await Page1.Locator("xpath=.//div[@class='_30jeq3 _16Jk6d']").First.ClickAsync();
            string prdPrice = await Page1.Locator(@"xpath=.//div[@class='_30jeq3 _16Jk6d']").InnerTextAsync();
            Assert.AreEqual(price, prdPrice);
        }

        [Test]
        public async Task Task6Test()
        {
            await Page.GotoAsync("https://www.saucedemo.com/");

            await Page.Locator("[data-test=\"username\"]").FillAsync("standard_user");

            await Page.Locator("[data-test=\"password\"]").FillAsync("secret_sauce");

            await Page.Locator("[data-test=\"login-button\"]").ClickAsync();

            string price = await Page.Locator("xpath=.//a[@id='item_4_title_link']/../..//div[@class='inventory_item_price']").InnerTextAsync();
            await Page.Locator(@"xpath=//button[@id='add-to-cart-sauce-labs-backpack']").ClickAsync();
            await Page.Locator(@"xpath=//div[@id='shopping_cart_container']").ClickAsync();
            string price2 = await Page.Locator(@"xpath=//div[@class='inventory_item_price']").InnerTextAsync();
            Assert.AreEqual(price, price2);
        }

        [Test]
        public async Task Task7Test()
        {
            await Page.GotoAsync("https://www.epam.com/");

            await Page.GetByRole(AriaRole.Button, new() { Name = "Search" }).ClickAsync();
            await Page.GetByRole(AriaRole.Option, new() { Name = "DevOps" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Find" }).ClickAsync();

            string foundCount = (await Page.Locator("xpath=.//h2[@class='search-results__counter']").InnerTextAsync());
            foundCount = foundCount.Replace(" RESULTS FOR \"DEVOPS\"", string.Empty);
            Console.WriteLine(foundCount);

            IReadOnlyList<ILocator> elements = await Page.Locator("xpath=.//article[@class='search-results__item']").AllAsync();
            foreach (ILocator locator in elements)
            {
                Assert.IsTrue((await locator.InnerTextAsync()).Contains("DevOps"));
            }
        }

        [Test]
        public async Task BrowserContextAuthManagementTest()
        {
            IBrowserContext context = await _driver.NewContextAsync();
            IPage page = await context.NewPageAsync();
            await Page.GotoAsync("https://www.saucedemo.com/");
            await Page.Locator("[data-test=\"username\"]").FillAsync("standard_user");
            await Page.Locator("[data-test=\"password\"]").FillAsync("secret_sauce");
            await Page.Locator("[data-test=\"login-button\"]").ClickAsync();
            List<Cookie> fakeCookies = new List<Cookie>();
            fakeCookies.Add(new Cookie() { Url = "https://www.saucedemo.com/", Name = "Key1", Value = "Test1" });
            await context.AddCookiesAsync(fakeCookies);

            string state = await context.StorageStateAsync(new BrowserContextStorageStateOptions() { Path = "state.json" });

            IBrowserContext context2 = await _driver.NewContextAsync();
            context2 = await _driver.NewContextAsync(new BrowserNewContextOptions() { StorageState = state });
            //context2 = await _driver.NewContextAsync(new BrowserNewContextOptions() { StorageStatePath = "state.json" });
            //await context2.AddInitScriptAsync(@"(storage => {
            //                                                    if (window.location.hostname === 'example.com') {
            //                                                      const entries = JSON.parse(storage);
            //                                                      for (const [key, value] of Object.entries(entries)) {
            //                                                        window.sessionStorage.setItem(key, value);
            //                                                      }
            //                                                    }
            //                                                  })('" + state + "')");
            IPage page2 = await context2.NewPageAsync();
            await page2.GotoAsync("https://www.saucedemo.com/");
            IReadOnlyList<BrowserContextCookiesResult> result = await context2.CookiesAsync();
            Assert.IsTrue(result.FirstOrDefault().Name.Equals("Key1"));
            Assert.IsTrue(result.FirstOrDefault().Value.Equals("Test1"));
        }

        [Test]
        public async Task ScreenPrintTest()
        {
            // RUn this test in headless mode. In headless mode also it takes screenshot .... new with playwright.
            await Page.GotoAsync("https://www.epam.com/");

            FileStream file = File.Create("screenPrint.png");
            file.Write(await Page.ScreenshotAsync());
            file.Close();

            Assert.IsTrue(File.Exists("screenPrint.png"));
        }

        [Test]
        public async Task ScreenVideoCaptureTest()
        {
            // RUn this test in headless mode. In headless mode also it takes videos .... new with playwright.
            Directory.Delete("videos",true);
            IBrowserContext context = await _driver.NewContextAsync(new() { RecordVideoDir = "videos/" });
            IPage thispage = await context.NewPageAsync();

            await thispage.GotoAsync("https://www.epam.com/");
            await thispage.GetByRole(AriaRole.Button, new() { Name = "Search" }).ClickAsync();
            await thispage.GetByRole(AriaRole.Option, new() { Name = "DevOps" }).ClickAsync();
            await thispage.GetByRole(AriaRole.Button, new() { Name = "Find" }).ClickAsync();

            await context.CloseAsync();

            Assert.IsTrue(Directory.Exists("videos"));
        }

        [Test]
        public async Task DynamicWebTablesTest()
        {
            // RUn this test in headless mode. In headless mode also it takes screenshot .... new with playwright.
            await Page.GotoAsync("https://testuserautomation.github.io/WebTable/");

            ILocator checkbox = Page.Locator("xpath=.//table//tbody//tr").Locator(":scope", new LocatorLocatorOptions() { HasText = "Ammy" }).Locator("[type='checkbox']");
            //checkboxList.Add(rows.Locator(":scope", new LocatorLocatorOptions() { HasText = "Ammy" }).Locator("[type='checkbox']"));
            
            await checkbox.ClickAsync();
            Assert.IsTrue(await checkbox.IsCheckedAsync());
        }

        [Test]
        public async Task PopupHandlingTest()
        {
            // RUn this test in headless mode. In headless mode also popup handling works the same 
            await Page.GotoAsync("http://autopract.com/selenium/popup/");
            await Page.Locator("a.open-button").ClickAsync();
            await Page.Locator("a.close-button").ClickAsync(); // Auto waiting for popup ....
        }

        [Test]
        public async Task TrishulTask()
        {
            var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            IBrowser browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            }) ;

            IPage myPage = await browser.NewPageAsync();

            await myPage.GotoAsync("https://www.guru99.com/");
            IReadOnlyList<ILocator> element = await myPage.Locator("xpath=.//b[text()='Testing']").AllAsync();
            string inntertext = await element.FirstOrDefault().InnerTextAsync();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            Task.Run(() => { Page.CloseAsync(); });            
            Task.Run(() => _driver.CloseAsync()).Wait();
            TestSession.Current.CleanUp();
        }

        private bool Getpassword(string pswd)
        {
            return pswd.Length >= 8 && pswd.Length <= 12;
        }
    }
}