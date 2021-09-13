using Core.Models;

namespace Infrastructure.Helpers.Specification
{
    public class GroupSpecification : ISpecification<Product>
    {
        private readonly Group _group;

        public GroupSpecification(Group group)
        {
            _group = group;
        }
        public bool IsSatisfied(Product t)
        {
            return t.GroupId == _group.Id;
        }
    }
}
