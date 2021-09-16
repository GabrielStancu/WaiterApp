using Core.Models;
using Infrastructure.Business.Wifi;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class WaiterRepository : GenericRepository<Waiter>, IWaiterRepository
    {
        public WaiterRepository(
            IWifiConnectionChecker wifiConnectionChecker, 
            IWifiConnectionResponseParser wifiConnectionResponseParser) 
            : base(wifiConnectionChecker, wifiConnectionResponseParser)
        {
        }

        public Waiter SelectWaiterWithCredentials(string username, string password)
        {
            using (var context = CreateContext())
            {
                var waiter = context.Waiter
                    .FirstOrDefault(w => w.Username == username && w.Password == password);

                if (waiter != null)
                {
                    waiter.Password = string.Empty;
                }

                return waiter;
            }
        }
    }
}
