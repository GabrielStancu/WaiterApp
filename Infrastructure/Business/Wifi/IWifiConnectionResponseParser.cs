namespace Infrastructure.Business.Wifi
{
    public interface IWifiConnectionResponseParser
    {
        string GenerateResponse(WifiConnectionResponse response);
    }
}