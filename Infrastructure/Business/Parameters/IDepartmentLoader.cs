using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Business.Parameters
{
    public interface IDepartmentLoader
    {
        List<Department> LoadAllDepartments();
        Department LoadCurrentDepartment(List<Department> departments, int departmentId);
    }
}