using Core.Models;

namespace Infrastructure.Business.Specification
{
    public class ProductNameSpecification : ISpecification<Product>
    {
        private readonly string _name;

        public ProductNameSpecification(string name)
        {
            _name = name;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Name.ToUpper().Contains(_name.ToUpper());
        }
    }
}
