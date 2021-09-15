using System.Collections.Generic;
using Xamarin.Essentials;

namespace Infrastructure.Business.Parameters
{
    public static class ParametersLoader
    {
        public static Dictionary<string, string> Parameters { get; private set; }
        public static void InitParameters()
        {
            Parameters = new Dictionary<string, string>
            {
                { AppParameters.Server, string.Empty },
                { AppParameters.Database, string.Empty },
                { AppParameters.DbUser, string.Empty },
                { AppParameters.DbPassword, string.Empty },

                { AppParameters.Username, string.Empty },
                { AppParameters.Password, string.Empty },
                { AppParameters.Remember, "false" },
                { AppParameters.WaiterId, "0" },

                { AppParameters.Nickname, string.Empty },
                { AppParameters.DepartmentId, "0" },
                { AppParameters.LoadDb, "false" },
                { AppParameters.ButtonsPerLine, "3" }
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
