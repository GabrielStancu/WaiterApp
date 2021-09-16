using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface ITableRepository: IGenericRepository<Table>
    {
        IEnumerable<Table> GetTablesForDepartment(int departmentId);
    }
}