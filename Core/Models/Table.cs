using Core.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Table : BaseModel
    {
        public int TableNumber { get; set; }
        public TableStatus Status { get; set; }
        [ForeignKey("Waiter")]
        public int WaiterId { get; set; }
        public Waiter Waiter { get; set; }
        public double Total { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int LengthX { get; set; }
        public int LengthY { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public override bool Equals(BaseModel other)
        {
            if (!(other is Table))
            {
                return false;
            }

            return other.Id == Id;
        }
    }
}
