using Core.Context;
using Core.Models;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> where T: BaseModel
    {
        protected RestaurantContext CreateContext()
        {
            try
            {
                var response = WifiConnectionChecker.CheckConnection();
                if (response == WifiConnectionResponse.WIFI_DATA_INTERNET)
                {
                    var restaurantDatabaseContext = (RestaurantContext)Activator.CreateInstance(typeof(RestaurantContext));
                    restaurantDatabaseContext.Database.EnsureCreated();
                    restaurantDatabaseContext.Database.Migrate();

                    return restaurantDatabaseContext;
                }
                else
                {
                    var parser = new WifiConnectionResponseParser();
                    string msg = parser.GenerateResponse(response);
                    throw new WifiConnectionException(msg);
                }
            }
            catch (SqlException)
            {
                throw new ConnectionStringException();
            }
        }

        public async Task<T> SelectByIdAsync(int id)
        {
            using (var context = CreateContext())
            {
                return await context.Set<T>().FindAsync(id);
            }  
        }

        public async Task<List<T>> SelectAllAsync()
        {
            using (var context = CreateContext())
            {
                return await context.Set<T>().ToListAsync();
            }   
        }

        public async Task InsertAsync(T entity)
        {
            using (var context = CreateContext())
            {
                bool alreadyStored = context.Set<T>().Any(e => e.Equals(entity));

                if (!alreadyStored)
                {
                    await context.Set<T>().AddAsync(entity);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateAsync(T entity)
        {
            using (var context = CreateContext())
            {
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
            }  
        }

        public async Task DeleteAsync(T entity)
        {
            using (var context = CreateContext())
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            } 
        }
    }
}
