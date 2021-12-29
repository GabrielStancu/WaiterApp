using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Subgrupa")]
    public class Subgroup : BaseModel
    {
        [Column("cods")]
        public new int Id { get; set; }
        [Column("denumire")]
        public string Name { get; set; }
        public Group Group { get; set; }
        [ForeignKey("Group")]
        [Column("codg")]
        public int GroupId { get; set; }
        public override bool Equals(BaseModel other)
        {
            if (other.GetType() != typeof(Subgroup))
            {
                return false;
            }

            if (Id > 0 && other.Id > 0)
            {
                return Id == other.Id;
            }

            Subgroup otherSubgroup = (Subgroup)other;

            return Name.ToUpper().Equals(otherSubgroup.Name.ToUpper())
                && Group == otherSubgroup.Group;
        }
    }
}
