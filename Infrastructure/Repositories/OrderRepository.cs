using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class OrderRepository: GenericRepository<Order>
    {
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
