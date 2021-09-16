using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface IGroupRepository: IGenericRepository<Group>
    {
        IEnumerable<Group> GetGroupsByDepartment(int departmentId);
    }
}