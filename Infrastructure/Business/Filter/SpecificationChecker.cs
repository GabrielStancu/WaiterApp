using Infrastructure.Business.Specification;
using System.Collections.Generic;

namespace Infrastructure.Business.Filter
{
    public  class SpecificationChecker<T> : ISpecificationChecker<T>
    {
        public IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec)
        {
            foreach (var item in items)
            {
                if (spec.IsSatisfied(item))
                {
                    yield return item;
                }
            }
        }
    }
}
