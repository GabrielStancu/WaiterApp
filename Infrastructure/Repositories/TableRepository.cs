using Core.Models;
using Infrastructure.Business.Wifi;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class TableRepository : GenericRepository<Table>, ITableRepository
    {
        public TableRepository(
            IWifiConnectionChecker wifiConnectionChecker, 
            IWifiConnectionResponseParser wifiConnectionResponseParser) 
            : base(wifiConnectionChecker, wifiConnectionResponseParser)
        {
        }

        public IEnumerable<Table> GetTablesForDepartment(int departmentId)
        {
            var tables = CreateContext()
                .Table
                .Where(t => t.DepartmentId == departmentId)
                .ToList();

            foreach (var table in tables)
            {
                if (table.WaiterId != 0)
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
