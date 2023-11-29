namespace EPAM.PlaywrightFW.Core.IOC.Containers;

[Serializable]
public class ServiceNotRegisteredException : Exception
{
    public ServiceNotRegisteredException() { }
    public ServiceNotRegisteredException(string message) : base(message) { }
    public ServiceNotRegisteredException(string message, Exception inner) : base(message, inner) { }
    protected ServiceNotRegisteredException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}