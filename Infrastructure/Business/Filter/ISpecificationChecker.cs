using Infrastructure.Business.Specification;
using System.Collections.Generic;

namespace Infrastructure.Business.Filter
{
    public  interface ISpecificationChecker<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }
}
