using Core.Models;
using Infrastructure.Business.Specification;
using System.Collections.Generic;

namespace Infrastructure.Business.Filter
{
    public class SubgroupFilter : ISubgroupFilter
    {
        public IEnumerable<Subgroup> Filter(IEnumerable<Subgroup> unfilteredSubgroups, Group group)
        {
            var specsList = new List<ISpecification<Subgroup>>
            {
                new SubgroupGroupSpecification(group)
            };

            var andSpecification = new AndSpecification<Subgroup>(specsList);
            var specificationChecker = new SpecificationChecker<Subgroup>();

            return specificationChecker.Filter(unfilteredSubgroups, andSpecification);
        }
    }
}
