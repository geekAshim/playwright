using EPAM.PlaywrightFW.Common;
using EPAM.PlaywrightFW.Core;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace EPAM.PlaywrightFW.Core.PlaywrightFactory;

public class FirefoxFactory : INamedBrowserFactory
{
    private readonly SessionSettings _options;
   
    public FirefoxFactory(SessionSettings options)
    {
        _options = options;
    }
    public async Task<IBrowser> CreateAsync()
    {
        var playwright = await Playwright.CreateAsync();
        return await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = _options.Headless
        });
    }
    public Browsers Name => Browsers.Firefox;
}