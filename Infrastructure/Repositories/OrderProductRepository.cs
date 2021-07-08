using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderProductRepository: GenericRepository<OrderProduct>
    {
        public async Task<IEnumerable<OrderProduct>> LoadOrdersForWaiterAsync(int waiterId)
        {
            return await CreateContext()
                .OrderProduct
                .Where(op => op.Order.WaiterId == waiterId && op.Product.IsRecipe)
                .Include(op => op.Product)
                .Include(op => op.Order)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderProduct>> LoadOrdersForTableAsync(int tableId)
        {
            return await CreateContext()
                .OrderProduct
                .Include(op => op.Product)
                .Include(op => op.Order).ThenInclude(o => o.Table)
                .Where(op => op.Order.TableId == tableId)
                .ToListAsync();
        }

        public async Task RegisterNewOrderProductAsync(OrderProduct orderProduct)
        {
            var insertOrderProduct = new OrderProduct()
            {
                OrderId = orderProduct.Id,
                PlacementTime = orderProduct.PlacementTime,
                ProductId = orderProduct.ProductId,
                Quantity = orderProduct.Quantity,
                ServingTime = orderProduct.ServingTime
            };
            await InsertAsync(insertOrderProduct);
        }
    }
}
