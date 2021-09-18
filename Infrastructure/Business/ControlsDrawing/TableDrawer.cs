using Core.Models;
using Infrastructure.Business.DeviceInfo;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Infrastructure.Business.ControlsDrawing
{
    public class TableDrawer : ITableDrawer
    {
        private readonly IDeviceInfoCollector _deviceInfoCollector;

        public TableDrawer(IDeviceInfoCollector deviceInfoCollector)
        {
            _deviceInfoCollector = deviceInfoCollector;
        }
        public IEnumerable<DrawnTable> DrawTables(IEnumerable<Table> tables, IEnumerable<Order> orders,
            int waiterId, int initialImageHeight, int initialImageWidth)
        {
            var drawnTables = new List<DrawnTable>();
            (double screenWidth, double screenHeight) = _deviceInfoCollector.GetScreenDimensions();
            double heightRatio = screenHeight / initialImageHeight;
            double widthRatio = screenWidth / initialImageWidth;
            int xOffset = (int)(screenWidth / 15);
            int yOffset = (int)(screenHeight / 15);

            foreach (var table in tables)
            {
                var drawnTable = new DrawnTable()
                {
                    TableNumber = table.TableNumber,
                    Color = GetTableColor(table, orders, waiterId),
                    Total = GetOrderTotal(table, orders),
                    WaiterName = table.Waiter?.FirstName,
                    StartX = (int)(table.StartX * widthRatio) - xOffset,
                    StartY = (int)(table.StartY * heightRatio) - yOffset,
                    LengthX = table.LengthX,
                    LengthY = table.LengthY
                };

                drawnTables.Add(drawnTable);
            }

            return drawnTables;
        }

        private double GetOrderTotal(Table table, IEnumerable<Order> orders)
        {
            var crtTableOrder = orders.FirstOrDefault(o => o.TableId == table.Id);

            return crtTableOrder?.Total ?? 0;
        }

        private Color GetTableColor(Table table, IEnumerable<Order> orders, int waiterId)
        {
            var crtTableOrder = orders.FirstOrDefault(o => o.TableId == table.Id && o.Paid == false);

            if (crtTableOrder is null)
            {
                return Color.Green;
            }

            if (waiterId == crtTableOrder.WaiterId)
            {
                return Color.FromHex("#922636");
            }

            return Color.Red;
        }
    }
}
