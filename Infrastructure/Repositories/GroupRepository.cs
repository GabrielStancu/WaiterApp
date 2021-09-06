using Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class GroupRepository: GenericRepository<Group>
    {
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
