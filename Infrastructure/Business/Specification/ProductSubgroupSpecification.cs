using Core.Models;

namespace Infrastructure.Business.Specification
{
    public class ProductSubgroupSpecification : ISpecification<Product>
    {
        private readonly Subgroup _subgroup;

        public ProductSubgroupSpecification(Subgroup subgroup)
        {
            _subgroup = subgroup;
        }
        public bool IsSatisfied(Product t)
        {
            return t.SubgroupId == _subgroup.Id || _subgroup.Id == 0;
        }
    }
}
