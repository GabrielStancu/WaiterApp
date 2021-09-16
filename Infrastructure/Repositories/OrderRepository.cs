using Core.Models;
using Infrastructure.Business.Wifi;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(
            IWifiConnectionChecker wifiConnectionChecker, 
            IWifiConnectionResponseParser wifiConnectionResponseParser) 
            : base(wifiConnectionChecker, wifiConnectionResponseParser)
        {
        }

        public IEnumerable<Order> LoadOrdersForDepartment(int departmentId)
        {
            return CreateContext()
                .Order
                .Include(o => o.Table)
                .Where(o => o.Paid == false && o.Table.DepartmentId == departmentId)
                .ToList();
        }
    }
}
