using Microsoft.Data.SqlClient;

namespace Infrastructure.Helpers
{
    public class ConnectionStringBuilder
    {
        private readonly string _server;
        private readonly string _database;
        private readonly string _user;
        private readonly string _password;

        public ConnectionStringBuilder(string server, string database, string user, string password)
        {
            _server = server;
            _database = database;
            _user = user;
            _password = password;
        }

        public string Build()
        {
            var connStrBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = _server ?? string.Empty,
                InitialCatalog = _database ?? string.Empty,
                UserID = _user ?? string.Empty,
                Password = _password ?? string.Empty
            };

            return connStrBuilder.ConnectionString;
        }
    }
}
