using Core.Models;
using Infrastructure.Helpers.Specification;
using System.Collections.Generic;

namespace Infrastructure.Helpers.Filter
{
    public class ProductsFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
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
