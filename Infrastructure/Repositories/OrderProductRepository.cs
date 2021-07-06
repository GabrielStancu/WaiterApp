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
            var orders = await CreateContext()
                .OrderProduct
                .Where(op => op.Order.WaiterId == waiterId && op.Product.IsRecipe)
                .Include(op => op.Product)
                .Include(op => op.Order)
                .ToListAsync();

            return orders;
        }
    }
}
