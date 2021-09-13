using Xamarin.Essentials;

namespace Infrastructure.Helpers.DeviceInfo
{
    public class DeviceInfoCollector
    {
        public (double Width, double Height) GetScreenDimensions()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            return (mainDisplayInfo.Width / mainDisplayInfo.Density, mainDisplayInfo.Height / mainDisplayInfo.Density);
        }
    }
}
