using Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ProductRepository: GenericRepository<Product>
    {
        public IEnumerable<Product> GetProductsByDepartment(int departmentId)
        {
            var products = CreateContext()
                .Product
                .Where(p => p.DepartmentId == departmentId)
                .ToList();

            return products;
        }
    }
}
