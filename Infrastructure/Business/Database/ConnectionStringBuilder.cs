using System.Text;

namespace Infrastructure.Business.Database
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
            var builder = new StringBuilder();
            builder.Append($"Data Source={_server}; ");
            builder.Append($"Initial Catalog={_database}; ");
            builder.Append($"User Id={_user}; ");
            builder.Append($"Password={_password};");

            return builder.ToString();
        }
    }
}
