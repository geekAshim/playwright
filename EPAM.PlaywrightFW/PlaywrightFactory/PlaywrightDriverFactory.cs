using EPAM.PlaywrightFW.Common;
using EPAM.PlaywrightFW.Core;
using EPAM.PlaywrightFW.Core.IOC.Containers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace EPAM.PlaywrightFW.Core.PlaywrightFactory;

public class PlaywrightDriverFactory
{
    private readonly  SessionSettings _driverOptions ;
    private readonly IServiceProvider _serviceProvider;

    public PlaywrightDriverFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _driverOptions = _serviceProvider.GetRequiredService<SessionSettings>();
    }

    public IBrowser Create()
    {
        var factory= _serviceProvider.GetServices<INamedBrowserFactory>()
            .FirstOrDefault(f=>f.Name == _driverOptions.Browser);
        if (factory == null)
        {
            throw new ServiceNotRegisteredException($"No factory registered for {_driverOptions.Browser} browser.");
        }
        return Task.Run(() => factory.CreateAsync()).Result;
    }
}
