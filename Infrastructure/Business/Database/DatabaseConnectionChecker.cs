using Core.Context;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Infrastructure.Exceptions;

namespace Infrastructure.Business.Database
{
    public class DatabaseConnectionChecker
    {
        public void TestConnection()
        {
            try
            {
                new ContextConnectionStringSetter().SetConnectionString();
                new RestaurantContext().Database.OpenConnection();
                new RestaurantContext().Database.CloseConnection();
            }
            catch(SqlException)
            {
                throw new ConnectionStringException();
            }
        }
    }
}
