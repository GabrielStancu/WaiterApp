using Core.Models;
using Infrastructure.Business.Specification;
using System.Collections.Generic;

namespace Infrastructure.Business.Filter
{
    public class ProductsFilter : IProductsFilter
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> unfilteredProducts,
            Group group, Subgroup subgroup, string name, string sequence)
        {
            var specsList = new List<ISpecification<Product>>
            {
                new ProductNameSpecification(name),
                new ProductSequenceSpecification(sequence)
            };

            if (group != null)
            {
                specsList.Add(new ProductGroupSpecification(group));
            }
            if (subgroup != null)
            {
                specsList.Add(new ProductSubgroupSpecification(subgroup));
            }

            var andSpecification = new AndSpecification<Product>(specsList);
            var specificationChecker = new SpecificationChecker<Product>();

            return specificationChecker.Filter(unfilteredProducts, andSpecification);
        }
    }
}
