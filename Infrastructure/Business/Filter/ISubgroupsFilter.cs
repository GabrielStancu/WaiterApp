using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Business.Filter
{
    public interface ISubgroupsFilter
    {
        IEnumerable<Subgroup> Filter(IEnumerable<Subgroup> unfilteredSubgroups, Group group);
    }
}