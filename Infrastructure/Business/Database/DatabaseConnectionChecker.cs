using Core.Context;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Infrastructure.Business.Database
{
    public class DatabaseConnectionChecker : IDatabaseConnectionChecker
    {
        private readonly IContextConnectionStringSetter _contextConnectionStringSetter;
        private RestaurantContext _restaurantContext;

        public DatabaseConnectionChecker(
            IContextConnectionStringSetter contextConnectionStringSetter)
        {
            _contextConnectionStringSetter = contextConnectionStringSetter;
            _restaurantContext = new RestaurantContext(); 
        }

        public bool TestConnection()
        {
            try
            {
                _contextConnectionStringSetter.SetConnectionString();
                _restaurantContext = new RestaurantContext();
                _restaurantContext.Database.OpenConnection();
                _restaurantContext.Database.CloseConnection();
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }
    }
}
