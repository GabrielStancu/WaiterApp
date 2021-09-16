using Core.Models;
using Infrastructure.Business.Wifi;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        public GroupRepository(
            IWifiConnectionChecker wifiConnectionChecker, 
            IWifiConnectionResponseParser wifiConnectionResponseParser) 
            : base(wifiConnectionChecker, wifiConnectionResponseParser)
        {
        }

        public IEnumerable<Group> GetGroupsByDepartment(int departmentId)
        {
            var groups = CreateContext()
                .Group
                .Where(g => g.DepartmentId == departmentId)
                .ToList();

            return groups;
        }
    }
}
