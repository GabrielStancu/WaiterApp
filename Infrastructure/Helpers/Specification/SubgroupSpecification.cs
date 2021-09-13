using Core.Models;

namespace Infrastructure.Helpers.Specification
{
    public class SubgroupSpecification : ISpecification<Product>
    {
        private readonly Subgroup _subgroup;

        public SubgroupSpecification(Subgroup subgroup)
        {
            _subgroup = subgroup;
        }
        public bool IsSatisfied(Product t)
        {
            return t.SubgroupId == _subgroup.Id;
        }
    }
}
