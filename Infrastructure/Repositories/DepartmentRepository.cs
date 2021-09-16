using Core.Models;
using Infrastructure.Business.Wifi;

namespace Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(
            IWifiConnectionChecker wifiConnectionChecker, 
            IWifiConnectionResponseParser wifiConnectionResponseParser) 
            : base(wifiConnectionChecker, wifiConnectionResponseParser)
        {
        }
    }
}
