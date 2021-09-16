using Core.Models;
using Infrastructure.Business.Wifi;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(
            IWifiConnectionChecker wifiConnectionChecker, 
            IWifiConnectionResponseParser wifiConnectionResponseParser) 
            : base(wifiConnectionChecker, wifiConnectionResponseParser)
        {
        }

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
