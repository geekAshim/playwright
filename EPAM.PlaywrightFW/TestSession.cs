using EPAM.PlaywrightFW.Common;
using EPAM.PlaywrightFW.Core.IOC.Containers;
using Microsoft.Extensions.DependencyInjection;

namespace EPAM.PlaywrightFW.Core;

public class TestSession
{
    private IServiceProvider _services = null!;

    public SessionSettings Settings => Resolve<SessionSettings>();

    public static TestSession Current => InstanceFactory.Value;

    private static readonly Lazy<TestSession> InstanceFactory = new(() => new TestSession());

    private TestSession()
    {
    }

    public void Start()
    {
        _services = ServiceRegistry.Register();
        if (!string.IsNullOrWhiteSpace(Settings.DownloadDirectory) && !Directory.Exists(Settings.DownloadDirectory))
        {
            Directory.CreateDirectory(Settings.DownloadDirectory);
        }
    }

    public void CleanUp()
    {
        if (Directory.Exists(Settings.DownloadDirectory))
        {
            Directory.Delete(Settings.DownloadDirectory, true);
        }
    }

    public T Resolve<T>() where T : notnull
    {
        if (_services == null)
        {
            throw new InvalidOperationException("The session is not started");
        }

        return _services.GetRequiredService<T>();
    }
}