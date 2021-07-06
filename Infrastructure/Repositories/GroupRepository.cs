using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GroupRepository: GenericRepository<Group>
    {
        public async Task<IEnumerable<Group>> GetGroupsByDepartmentAsync(int departmentId)
        {
            var groups = await CreateContext()
                .Group
                .Where(g => g.DepartmentId == departmentId)
                .ToListAsync();

            return groups;
        }
    }
}
