using Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SubgroupRepository: GenericRepository<Subgroup>
    {
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
