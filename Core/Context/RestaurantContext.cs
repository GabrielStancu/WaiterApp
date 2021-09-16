using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Context
{
    public class RestaurantContext : DbContext
    {
        public DbSet<Product> Product { get; private set; }
        public DbSet<Group> Group { get; private set; }
        public DbSet<Subgroup> Subgroup { get; private set; }
        public DbSet<Department> Department { get; private set; }
        public DbSet<Waiter> Waiter { get; private set; }
        public DbSet<OrderProduct> OrderProduct { get; private set; }
        public DbSet<Order> Order { get; private set; }
        public DbSet<Table> Table { get; private set; }

        public static string ConnectionString = string.Empty;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(ConnectionString);
    }
}
