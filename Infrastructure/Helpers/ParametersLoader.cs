using System.Collections.Generic;
using Xamarin.Essentials;

namespace Infrastructure.Helpers
{
    public class ParametersLoader
    {
        public Dictionary<string, string> Parameters { get; private set; }

        public ParametersLoader()
        {
            InitParameters();
            LoadParameters();
        }

        public void SaveParameters()
        {
            foreach (var paramKey in Parameters.Keys)
            {
                Preferences.Set(paramKey, Parameters[paramKey]);
            }
        }

        public void StoreParameters()
        {
            foreach (var paramKey in Parameters.Keys)
            {
                if(!Preferences.ContainsKey(paramKey))
                {
                    Preferences.Set(paramKey, Parameters[paramKey]);
                }
            }
        }

        public void SetParameter(string paramKey, string paramValue)
        {
            Parameters[paramKey] = paramValue;
        }

        public string GetParameter(string paramKey)
        {
            return Preferences.Get(paramKey, string.Empty);
        }

        private void InitParameters()
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

        private void LoadParameters()
        {
            var parameters = new Dictionary<string, string>();
            foreach (var paramKey in Parameters.Keys)
            {
                parameters.Add(paramKey, Preferences.Get(paramKey, string.Empty));
            }

            Parameters = new Dictionary<string, string>(parameters);
        }
    }
}
