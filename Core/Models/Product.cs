using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("MARFA")]
    public class Product : BaseModel
    {
        [Column("secventa")]
        public new int Id { get; set; }
        [Column("secventa")]
        public string Sequence { get; set; }
        [Column("denumire")]
        public string Name { get; set; }
        public Group Group { get; set; }
        [ForeignKey("Group")]
        [Column("cod grupa")]
        public int GroupId { get; set; }
        public Subgroup Subgroup { get; set; }
        [ForeignKey("Subgroup")]
        [Column("cod subgrupa")]
        public int SubgroupId { get; set; }
        [Column("pret vanzare")]
        public double Price { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Department")]
        [Column("cod gestiune")]
        public int DepartmentId { get; set; }
        [Column("NO-NAME")]
        public bool IsRecipe { get; set; }
        [Column("cantitate")]
        private double _stock;
        public double Stock 
        {
            get => _stock;
            set
            {
                _stock = value;
                SetProperty<double>(ref _stock, value);
            }
        }
        [NotMapped]
        public string ProductNameWithPrice { get => $"{Name}\n{Price} [{Stock}]"; }

        public override bool Equals(BaseModel other)
        {
            if (other.GetType() != typeof(Product))
            {
                return false;
            }

            Product otherProduct = (Product)other;

            if (Id > 0 && other.Id > 0)
            {
                return Id == other.Id;
            }

            return Name.ToUpper().Equals(otherProduct.Name.ToUpper())
                && Group == otherProduct.Group
                && Subgroup == otherProduct.Subgroup
                && Department == otherProduct.Department
                && Price == otherProduct.Price;
        }
    }
}
