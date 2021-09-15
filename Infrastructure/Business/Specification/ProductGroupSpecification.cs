using Core.Models;

namespace Infrastructure.Business.Specification
{
    public class ProductGroupSpecification : ISpecification<Product>
    {
        private readonly Group _group;

        public ProductGroupSpecification(Group group)
        {
            _group = group;
        }
        public bool IsSatisfied(Product t)
        {
            return t.GroupId == _group.Id || _group.Id == 0;
        }
    }
}
