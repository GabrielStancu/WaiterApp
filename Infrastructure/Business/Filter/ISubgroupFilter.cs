using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Business.Filter
{
    public interface ISubgroupFilter
    {
        IEnumerable<Subgroup> Filter(IEnumerable<Subgroup> unfilteredSubgroups, Group group);
    }
}