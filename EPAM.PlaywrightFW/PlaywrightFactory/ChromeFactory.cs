
using EPAM.PlaywrightFW.Common;
using Microsoft.Playwright;

namespace EPAM.PlaywrightFW.Core.PlaywrightFactory;

public class ChromeFactory : INamedBrowserFactory
{
    private readonly SessionSettings _options;
    IPage Page;
    public ChromeFactory(SessionSettings options)
    {
        _options = options;
    }
    public async Task<IBrowser> CreateAsync()
    {
        var playwright = await Playwright.CreateAsync();
        return await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = _options.Headless
        });        
    }

    public Browsers Name => Browsers.Chrome;
    
}