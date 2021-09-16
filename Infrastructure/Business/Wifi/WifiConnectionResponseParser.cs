namespace Infrastructure.Business.Wifi
{
    public class WifiConnectionResponseParser : IWifiConnectionResponseParser
    {
        public string GenerateResponse(WifiConnectionResponse response)
        {
            switch (response)
            {
                case WifiConnectionResponse.NO_INTERNET:
                    return "No internet connection!";
                case WifiConnectionResponse.WIFI_DATA_INTERNET:
                    return string.Empty;
                case WifiConnectionResponse.OTHER_CONNECTION:
                    return "You are not connected to the wifi network!";
                default:
                    return "Could not determine the connection state!";
            }
        }
    }
}
