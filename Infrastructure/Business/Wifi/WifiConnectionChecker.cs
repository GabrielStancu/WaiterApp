using System.Collections.Generic;
using Xamarin.Essentials;

namespace Infrastructure.Business.Wifi
{
    public class WifiConnectionChecker
    {
        public static WifiConnectionResponse CheckConnection()
        {
            var networkAccess = Connectivity.NetworkAccess;

            if (networkAccess == NetworkAccess.Internet)
            {
                IEnumerable<ConnectionProfile> networkProfiles = Connectivity.ConnectionProfiles;

                foreach (var profile in networkProfiles)
                {
                    if (profile.Equals(ConnectionProfile.WiFi))
                    {
                        return WifiConnectionResponse.WIFI_DATA_INTERNET;
                    }
                }

                return WifiConnectionResponse.OTHER_CONNECTION;
            }
            else
            {
                return WifiConnectionResponse.NO_INTERNET;
            }
        }
    }
}
