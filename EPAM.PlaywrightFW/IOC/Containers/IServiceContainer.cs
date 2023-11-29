using Microsoft.Extensions.DependencyInjection;

namespace EPAM.PlaywrightFW.Core.IOC.Containers;

public interface IServiceContainer
{
    void Register(IServiceCollection collection);
}