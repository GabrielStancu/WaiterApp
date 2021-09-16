using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface IOrderRepository: IGenericRepository<Order>
    {
        IEnumerable<Order> LoadOrdersForDepartment(int departmentId);
    }
}