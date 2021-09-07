using System.Collections.Generic;
using Xamarin.Essentials;

namespace Infrastructure.Helpers
{
    public static class ParametersLoader
    {
        public static Dictionary<string, string> Parameters { get; private set; }
        public static void InitParameters()
        {
            Parameters = new Dictionary<string, string>
            {
                { "server", string.Empty },
                { "database", string.Empty },
                { "dbUser", string.Empty },
                { "dbPassword", string.Empty },

                { "username", string.Empty },
                { "password", string.Empty },
                { "remember", "false" },
                { "waiterId", "0" },

                { "nickname", string.Empty },
                { "departmentId", "0" },
                { "loadDb", "false" },
                { "buttonsPerLine", "3" }
            };

            StoreParameters();
        }

        public static void LoadParameters()
        {
            var parameters = new Dictionary<string, string>();
            foreach (var paramKey in Parameters.Keys)
            {
                parameters.Add(paramKey, Preferences.Get(paramKey, string.Empty));
            }

            Parameters = new Dictionary<string, string>(parameters);
        }
        public static void SaveParameters()
        {
            foreach (var paramKey in Parameters.Keys)
            {
                Preferences.Set(paramKey, Parameters[paramKey]);
            }
        }

        public static void StoreParameters()
        {
            foreach (var paramKey in Parameters.Keys)
            {
                if(!Preferences.ContainsKey(paramKey))
                {
                    Preferences.Set(paramKey, Parameters[paramKey]);
                }
            }
        }

        public static void SetParameter(string paramKey, string paramValue)
        {
            Parameters[paramKey] = paramValue;
        }

        
    }
}
