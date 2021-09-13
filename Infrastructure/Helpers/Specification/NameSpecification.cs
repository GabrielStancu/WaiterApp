using Core.Models;

namespace Infrastructure.Helpers.Specification
{
    public class NameSpecification : ISpecification<Product>
    {
        private readonly string _name;

        public NameSpecification(string name)
        {
            _name = name;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Name.Equals(_name);
        }
    }
}
