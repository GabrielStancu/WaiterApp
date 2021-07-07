using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TableRepository: GenericRepository<Table>
    {
        public async Task<IEnumerable<Table>> GetTablesForDepartmentAsync(int departmentId)
        {
            var tables = await CreateContext()
                .Table
                .Where(t => t.DepartmentId == departmentId)
                .ToListAsync();

            foreach (var table in tables)
            {
                if(table.WaiterId != 0)
                {
                    table.Waiter =
                        await CreateContext().Waiter
                                .FirstOrDefaultAsync(w => w.Id == table.WaiterId);
                }
            }

            return tables;
        }
    }
}
