using Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class TableRepository: GenericRepository<Table>
    {
        public IEnumerable<Table> GetTablesForDepartment(int departmentId)
        {
            var tables = CreateContext()
                .Table
                .Where(t => t.DepartmentId == departmentId)
                .ToList();

            foreach (var table in tables)
            {
                if(table.WaiterId != 0)
                {
                    table.Waiter =
                        CreateContext().Waiter
                                .FirstOrDefault(w => w.Id == table.WaiterId);
                }
            }

            return tables;
        }
    }
}
