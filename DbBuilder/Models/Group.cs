using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Group : BaseModel
    {
        public string Name { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Department")]
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
