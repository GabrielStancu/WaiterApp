using Core.Context;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Infrastructure.Exceptions;
using System;

namespace Infrastructure.Business.Database
{
    public class DatabaseConnectionChecker : IDatabaseConnectionChecker
    {
        private readonly IContextConnectionStringSetter _contextConnectionStringSetter;
        private readonly RestaurantContext _restaurantContext;

        public DatabaseConnectionChecker(IContextConnectionStringSetter contextConnectionStringSetter)
        {
            _contextConnectionStringSetter = contextConnectionStringSetter;
            _restaurantContext = (RestaurantContext)Activator.CreateInstance(typeof(RestaurantContext)); 
        }
        public void TestConnection()
        {
            try
            {
                _contextConnectionStringSetter.SetConnectionString();
                _restaurantContext.Database.OpenConnection();
                _restaurantContext.Database.CloseConnection();
            }
            catch (SqlException)
            {
                throw new ConnectionStringException();
            }
        }
    }
}
