using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Subgroup : BaseModel
    {
        public string Name { get; set; }
        public Group Group { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

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
                && Group == otherSubgroup.Group
                && Department == otherSubgroup.Department;
        }
    }
}
