using System.Text;

namespace Infrastructure.Business.Database
{
    public class ConnectionStringBuilder : IConnectionStringBuilder
    {
        public string Build(string server, string database, string user, string password)
        {
            var builder = new StringBuilder();
            builder.Append($"Data Source={server}; ");
            builder.Append($"Initial Catalog={database}; ");
            builder.Append($"User Id={user}; ");
            builder.Append($"Password={password};");

            return builder.ToString();
        }
    }
}
