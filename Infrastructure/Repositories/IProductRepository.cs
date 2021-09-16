using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface IProductRepository: IGenericRepository<Product>
    {
        IEnumerable<Product> GetProductsByDepartment(int departmentId);
    }
}