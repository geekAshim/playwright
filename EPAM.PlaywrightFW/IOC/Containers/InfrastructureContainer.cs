using System.Diagnostics.CodeAnalysis;
using EPAM.PlaywrightFW.Common;
using EPAM.PlaywrightFW.Core.PlaywrightFactory;
using Microsoft.Extensions.DependencyInjection;

namespace EPAM.PlaywrightFW.Core.IOC.Containers;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
public class InfrastructureContainer : IServiceContainer
{
    public void Register(IServiceCollection collection)
    {
        collection
            .AddOptions()
            .AddHttpClient()
            .UseTestConfiguration<SessionSettings>()
            .AddSingleton<INamedBrowserFactory, ChromeFactory>()
            .AddSingleton<INamedBrowserFactory, FirefoxFactory>()
            .AddSingleton<INamedBrowserFactory, WebKitFactory>()
            .AddTransient(s => new PlaywrightDriverFactory(s).Create());            
    }
}