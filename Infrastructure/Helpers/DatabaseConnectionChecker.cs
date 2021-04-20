using Core.Context;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Infrastructure.Exceptions;

namespace Infrastructure.Helpers
{
    public class DatabaseConnectionChecker
    {
        public void TestConnection()
        {
            try
            {
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
