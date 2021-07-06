using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public Group Group { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public Subgroup Subgroup { get; set; }
        [ForeignKey("Subgroup")]
        public int SubgroupId { get; set; }
        public decimal Price { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public bool IsRecipe { get; set; }

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
