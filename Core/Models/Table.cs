using Core.Business;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Table : BaseModel
    {
        public int TableNumber { get; set; }
        [ForeignKey("Waiter")]
        public int? WaiterId { get; set; }
        public Waiter Waiter { get; set; }
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
        public TableStatus GetStatus(int waiterId)
        {
            if (WaiterId is null)
                return TableStatus.Free;
            if (WaiterId == waiterId)
                return TableStatus.TakenByCurrentWaiter;
            return TableStatus.TakenByOtherWaiter;
        }
    }
}
