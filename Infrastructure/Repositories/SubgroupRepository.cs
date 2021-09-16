using Core.Models;
using Infrastructure.Business.Wifi;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SubgroupRepository : GenericRepository<Subgroup>, ISubgroupRepository
    {
        public SubgroupRepository(
            IWifiConnectionChecker wifiConnectionChecker, 
            IWifiConnectionResponseParser wifiConnectionResponseParser) 
            : base(wifiConnectionChecker, wifiConnectionResponseParser)
        {
        }

        public IEnumerable<Subgroup> GetSubgroupsByDepartment(int departmentId)
        {
            var subgroups = CreateContext()
                .Subgroup
                .Where(s => s.DepartmentId == departmentId)
                .ToList();

            return subgroups;
        }
    }
}
