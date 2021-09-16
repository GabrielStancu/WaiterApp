using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Business.TablesDrawing
{
    public interface ITableDrawer
    {
        IEnumerable<DrawnTable> DrawTables(IEnumerable<Table> tables, IEnumerable<Order> orders, int waiterId, int initialImageHeight, int initialImageWidth);
    }
}