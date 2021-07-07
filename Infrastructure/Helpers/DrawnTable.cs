using System.Drawing;

namespace Infrastructure.Helpers
{
    public class DrawnTable
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int LengthX { get; set; }
        public int LengthY { get; set; }
        public Color Color { get; set; }
        public string WaiterName { get; set; }
        public double Total { get; set; }
    }
}
