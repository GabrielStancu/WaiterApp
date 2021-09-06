using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class OrderProductRepository: GenericRepository<OrderProduct>
    {
        public IEnumerable<OrderProduct> LoadOrdersForWaiter(int waiterId)
        {
            return CreateContext()
                .OrderProduct
                .Where(op => op.Order.WaiterId == waiterId && op.Product.IsRecipe)
                .Include(op => op.Product)
                .Include(op => op.Order)
                .ToList();
        }

        public IEnumerable<OrderProduct> LoadOrdersForTable(int tableId)
        {
            return CreateContext()
                .OrderProduct
                .Include(op => op.Product)
                .Include(op => op.Order).ThenInclude(o => o.Table)
                .Where(op => op.Order.TableId == tableId)
                .ToList();
        }

        public void RegisterNewOrderProduct(OrderProduct orderProduct)
        {
            var insertOrderProduct = new OrderProduct()
            {
                OrderId = orderProduct.OrderId,
                PlacementTime = orderProduct.PlacementTime,
                ProductId = orderProduct.ProductId,
                Quantity = orderProduct.Quantity,
                ServingTime = orderProduct.ServingTime
            };
            Insert(insertOrderProduct);
        }
    }
}
