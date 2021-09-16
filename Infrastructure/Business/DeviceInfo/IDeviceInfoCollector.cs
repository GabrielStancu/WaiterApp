namespace Infrastructure.Business.DeviceInfo
{
    public interface IDeviceInfoCollector
    {
        (double Width, double Height) GetScreenDimensions();
    }
}