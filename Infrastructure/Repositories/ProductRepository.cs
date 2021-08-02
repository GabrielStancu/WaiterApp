using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository: GenericRepository<Product>
    {
        public async Task<IEnumerable<Product>> GetProductsByDepartmentAsync(int departmentId)
        {
            var products = await CreateContext()
                .Product
                .Where(p => p.DepartmentId == departmentId)
                .ToListAsync();

            return products;
        }
    }
}
