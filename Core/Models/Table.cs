using Core.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Table : BaseModel
    {
        public int TableNumber { get; set; }
        public TableStatus TableStatus { get; set; }
        [ForeignKey("Waiter")]
        public int WaiterId { get; set; }
        public Waiter Waiter { get; set; }
        public float Total { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int LengthX { get; set; }
        public int LegnthY { get; set; }

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
