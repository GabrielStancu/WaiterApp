using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Grupa")]
    public class Group : BaseModel
    {
        [Column("codg")]
        public new int Id { get; set; }
        [Column("denumire")]
        public string Name { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Department")]
        [Column("NO-NAME")]
        public int DepartmentId { get; set; }

        public override bool Equals(BaseModel other)
        {
            if (other.GetType() != typeof(Group))
            {
                return false;
            }

            if (Id > 0 && other.Id > 0)
            {
                return Id == other.Id;
            }

            Group otherGroup = (Group)other;

            return Name.ToUpper().Equals(otherGroup.Name.ToUpper())
                && Department == otherGroup.Department;
        }
    }
}
