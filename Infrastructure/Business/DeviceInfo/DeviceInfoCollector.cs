using Xamarin.Essentials;

namespace Infrastructure.Business.DeviceInfo
{
    public class DeviceInfoCollector : IDeviceInfoCollector
    {
        public (double Width, double Height) GetScreenDimensions()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            return (mainDisplayInfo.Width / mainDisplayInfo.Density, mainDisplayInfo.Height / mainDisplayInfo.Density);
        }
    }
}
