using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SubgroupRepository: GenericRepository<Subgroup>
    {
        public async Task<IEnumerable<Subgroup>> GetSubgroupsByDepartmentAsync(int departmentId)
        {
            var subgroups = await CreateContext()
                .Subgroup
                .Where(s => s.DepartmentId == departmentId)
                .ToListAsync();

            return subgroups;
        }
    }
}
