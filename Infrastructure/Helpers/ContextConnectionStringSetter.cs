using Core.Context;

namespace Infrastructure.Helpers
{
    public class ContextConnectionStringSetter
    {
        public void SetConnectionString()
        {
            var server = ParametersLoader.Parameters["server"];
            var database = ParametersLoader.Parameters["database"];
            var user = ParametersLoader.Parameters["dbUser"];
            var password = ParametersLoader.Parameters["dbPassword"];

            var connStrBuilder = new ConnectionStringBuilder(server, database, user, password);
            RestaurantContext.ConnectionString = connStrBuilder.Build();
        }
    }
}
