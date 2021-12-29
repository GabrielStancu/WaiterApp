using Core.Business;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Mese")]
    public class Table : BaseModel
    {
        [Column("ID_masa")]
        public new int Id { get; set; }
        [Column("Numar")]
        public int TableNumber { get; set; }
        [ForeignKey("Waiter")]
        [Column("ID_Ospatar")]
        public int? WaiterId { get; set; }
        public Waiter Waiter { get; set; }
        [Column("poz_x_ts")]
        public int StartX { get; set; }
        [Column("poz_y_ts")]
        public int StartY { get; set; }
        [Column("latime_ts")]
        public int LengthX { get; set; }
        [Column("lungime_ts")]
        public int LengthY { get; set; }
        [ForeignKey("Department")]
        [Column("cod gestiune")]
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
