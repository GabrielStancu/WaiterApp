using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface IOrderProductRepository: IGenericRepository<OrderProduct>
    {
        IEnumerable<OrderProduct> LoadOrdersForTable(int tableId);
        IEnumerable<OrderProduct> LoadOrdersForWaiter(int waiterId);
        IEnumerable<OrderProduct> LoadAllOrdersForWaiter(int waiterId);
        void RegisterNewOrderProduct(OrderProduct orderProduct);
    }
}