using Infrastructure.Helpers.Specification;
using System.Collections.Generic;

namespace Infrastructure.Helpers.Filter
{
    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }
}
