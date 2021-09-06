using Core.Context;
using Core.Models;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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

        public T SelectById(int id)
        {
            using (var context = CreateContext())
            {
                return context.Set<T>().Find(id);
            }  
        }

        public List<T> SelectAll()
        {
            using (var context = CreateContext())
            {
                return context.Set<T>().ToList();
            }   
        }

        public void Insert(T entity)
        {
            using (var context = CreateContext())
            {
                bool alreadyStored = context.Set<T>().Any(e => e.Equals(entity));

                if (!alreadyStored)
                {
                    context.Set<T>().Add(entity);
                    context.SaveChanges();
                }
            }
        }

        public void Update(T entity)
        {
            using (var context = CreateContext())
            {
                context.Set<T>().Update(entity);
                context.SaveChanges();
            }  
        }

        public void Delete(T entity)
        {
            using (var context = CreateContext())
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            } 
        }
    }
}
