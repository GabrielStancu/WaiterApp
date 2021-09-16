namespace Infrastructure.Business.Wifi
{
    public interface IWifiConnectionChecker
    {
        WifiConnectionResponse CheckConnection();
    }
}