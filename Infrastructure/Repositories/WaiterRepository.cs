using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class WaiterRepository: GenericRepository<Waiter>
    {
        public async Task<Waiter> SelectWaiterWithCredentialsAsync(string username, string password)
        {
            using (var context = CreateContext())
            {
                var waiter = await context.Waiter
                    .FirstOrDefaultAsync(w => w.Username == username && w.Password == password);
                    
                if(waiter != null)
                {
                    waiter.Password = string.Empty;
                }

                return waiter;
            }
        }
    }
}
