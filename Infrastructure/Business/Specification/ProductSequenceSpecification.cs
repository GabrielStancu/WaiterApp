using Core.Models;

namespace Infrastructure.Business.Specification
{
    public class ProductSequenceSpecification : ISpecification<Product>
    {
        private readonly string _sequence;

        public ProductSequenceSpecification(string sequence)
        {
            _sequence = sequence;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Sequence.ToUpper().Contains(_sequence.ToUpper());
        }
    }
}
