using EPAM.PlaywrightFW.Core;
using Microsoft.Playwright;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SpecFlowTest.StepDefinitions
{
    [Binding]
    public class SingleSteps
    {
        private IBrowser? _driver;
        IPage Page;

        public SingleSteps()
        {
            TestSession.Current.Start();
            _driver = Task.Run(() => TestSession.Current.Resolve<IBrowser>()).Result;
            var url = TestSession.Current.Settings.ApplicationUrl;
            Page = Task.Run(() => _driver.NewPageAsync()).Result;
        }

        [Given(@"I am on ""(.*)""")]
        public async Task GivenIAmOnTheGooglePageAsync(string url)
        {
            await Page.GotoAsync(url);
        }

        [When(@"I search for ""(.*)""")]
        public async Task WhenISearchForAsync(string keyword)
        {
            await Page.FillAsync("[title='Search']", keyword);
            await Page.ClickAsync("[value='Google Search'] >> nth=1");
        }

        [Then(@"I should see result ""(.*)""")]
        public async Task ThenIShouldSeeResultAsync(string result)
        {
            var firstResult = await Page.Locator("//cite >> nth=0").InnerTextAsync();
            Assert.AreEqual(result, firstResult);
        }
    }
}