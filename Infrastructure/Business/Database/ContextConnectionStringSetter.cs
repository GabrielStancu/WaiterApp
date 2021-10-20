using Core.Context;
using Infrastructure.Business.Parameters;
using System;

namespace Infrastructure.Business.Database
{
    public class ContextConnectionStringSetter : IContextConnectionStringSetter
    {
        private readonly IConnectionStringBuilder _connectionStringBuilder;

        public ContextConnectionStringSetter(IConnectionStringBuilder connectionStringBuilder)
        {
            _connectionStringBuilder = connectionStringBuilder;
        }
        public void SetConnectionString()
        {
            var server = ParametersLoader.Parameters[AppParameters.Server];
            var database = ParametersLoader.Parameters[AppParameters.Database];
            var user = ParametersLoader.Parameters[AppParameters.DbUser];
            var password = ParametersLoader.Parameters[AppParameters.DbPassword];

            RestaurantContext.ConnectionString = _connectionStringBuilder.Build(server, database, user, password);
        }
    }
}
