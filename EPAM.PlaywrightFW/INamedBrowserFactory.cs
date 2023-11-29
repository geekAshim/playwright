using EPAM.PlaywrightFW.Common;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace EPAM.PlaywrightFW.Core;
public interface INamedBrowserFactory : IFactory<IBrowser>
{
    Browsers Name { get; }
    //Task<IBrowser> CreateAsync();    
}

public interface IFactory<T>
{
    Task<T> CreateAsync();
}