using Core.Context;

namespace Infrastructure.Helpers
{
    public class ContextConnectionStringSetter
    {
        public void SetConnectionString()
        {
            var paramLoader = new ParametersLoader();

            var server = paramLoader.Parameters["server"];
            var database = paramLoader.Parameters["database"];
            var user = paramLoader.Parameters["dbUser"];
            var password = paramLoader.Parameters["dbPassword"];

            var connStrBuilder = new ConnectionStringBuilder(server, database, user, password);
            RestaurantContext.ConnectionString = connStrBuilder.Build();
        }
    }
}
