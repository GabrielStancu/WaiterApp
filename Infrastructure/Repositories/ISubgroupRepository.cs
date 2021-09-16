using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface ISubgroupRepository: IGenericRepository<Subgroup>
    {
        IEnumerable<Subgroup> GetSubgroupsByDepartment(int departmentId);
    }
}