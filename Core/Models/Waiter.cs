using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Ospatari")]
    public class Waiter: BaseModel
    {
        [Column("ID_ospatar")]
        public new int Id { get; set; }
        [Column("Nick")]
        public string Nickname { get; set; }
        [Column("Nume")]
        public string Username { get; set; }
        [Column("Password")]
        public string Password { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Department")]
        [Column("divizie_id")]
        public int DepartmentId { get; set; }

        public override bool Equals(BaseModel other)
        {
            if (other.GetType() != typeof(Waiter))
            {
                return false;
            }

            if (Id > 0 && other.Id > 0)
            {
                return Id == other.Id;
            }

            Waiter otherUser = (Waiter)other;

            return Username.ToUpper().Equals(otherUser.Username.ToUpper())
                && Department == otherUser.Department;
        }
    }
}
