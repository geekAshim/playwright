using Microsoft.Extensions.DependencyInjection;

namespace EPAM.PlaywrightFW.Core.IOC.Containers;

public static class  ServiceRegistry
{
    public static IServiceProvider Register()
    {
        var collection = new ServiceCollection();
        collection.RegisterContainers();
        return collection.BuildServiceProvider();
    }
    
}