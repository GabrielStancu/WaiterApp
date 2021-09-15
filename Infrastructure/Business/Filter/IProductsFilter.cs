using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Business.Filter
{
    public interface IProductsFilter
    {
        IEnumerable<Product> Filter(IEnumerable<Product> unfilteredProducts, Group group, Subgroup subgroup, string name, string sequence);
    }
}