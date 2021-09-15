using Core.Models;

namespace Infrastructure.Business.Specification
{
    public class SubgroupGroupSpecification : ISpecification<Subgroup>
    {
        private readonly Group _group;

        public SubgroupGroupSpecification(Group group)
        {
            _group = group;
        }
        public bool IsSatisfied(Subgroup t)
        {
            return t.GroupId == _group.Id || _group.Id == 0;
        }
    }
}
