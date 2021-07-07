using Core.Helpers;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Infrastructure.Helpers
{
    public class TableDrawer
    {
        public IEnumerable<DrawnTable> DrawTables(IEnumerable<Table> tables, 
            int initialImageHeight, int initialImageWidth)
        {          
            var drawnTables = new List<DrawnTable>();
            (double screenWidth, double screenHeight) = new DeviceInfoCollector().GetScreenDimensions();
            double heightRatio = screenHeight / initialImageHeight;
            double widthRatio = screenWidth / initialImageWidth;
            int xOffset = (int)(screenWidth / 15);
            int yOffset = (int)(screenHeight / 15);

            foreach (var table  in tables)
            {
                var drawnTable = new DrawnTable()
                {
                    Color = GetColorByStatus(table.Status),
                    Total = table.Total,
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

        private Color GetColorByStatus(TableStatus status)
        {
            switch(status)
            {
                case TableStatus.Free:
                    return Color.Green;
                case TableStatus.TakenByCurrentWaiter:
                    return Color.Blue;
                case TableStatus.TakenByOtherWaiter:
                    return Color.Red;
                default:
                    return Color.Gray;
            }
        }
    }
}
