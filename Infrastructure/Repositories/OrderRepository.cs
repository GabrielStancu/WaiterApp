using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository: GenericRepository<Order>
    {
        public async Task<IEnumerable<Order>> LoadOrdersForDepartmentAsync(int departmentId)
        {
            return await CreateContext()
                .Order
                .Include(o => o.Table)
                .Where(o => o.Paid == false && o.Table.DepartmentId == departmentId)
                .ToListAsync();
        }
    }
}
