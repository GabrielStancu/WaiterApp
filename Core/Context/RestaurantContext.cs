using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Context
{
    public class RestaurantContext: DbContext
    {
        public DbSet<Product> Product { get; private set; }
        public DbSet<Group> Group { get; private set; }
        public DbSet<Subgroup> Subgroup { get; private set; }
        public DbSet<Department> Department { get; private set; }
        public DbSet<Waiter> Waiter { get; private set; }
        public DbSet<OrderProduct> OrderProduct { get; private set; }
        public DbSet<Order> Order { get; private set; }

        private static string _connectionString = "Data Source=192.168.100.2;Initial Catalog=RestaurantDb;persist security info=True; Integrated Security = SSPI;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_connectionString);

        public RestaurantContext(string connectionString)
            => _connectionString = connectionString;

    }
}
