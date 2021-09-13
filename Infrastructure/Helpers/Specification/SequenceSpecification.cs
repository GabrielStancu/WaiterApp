using Core.Models;

namespace Infrastructure.Helpers.Specification
{
    public class SequenceSpecification : ISpecification<Product>
    {
        private readonly string _sequence;

        public SequenceSpecification(string sequence)
        {
            _sequence = sequence;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Sequence.Equals(_sequence);
        }
    }
}
