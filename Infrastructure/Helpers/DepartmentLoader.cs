using Core.Models;
using Infrastructure.Repositories;
using System.Collections.Generic;

namespace Infrastructure.Helpers
{
    public class DepartmentLoader
    {
        public List<Department> LoadAllDepartments()
        {
            var departmentRepository = new DepartmentRepository();
            return departmentRepository.SelectAll();
        }

        public Department LoadCurrentDepartment(List<Department> departments, int departmentId)
        {
            foreach (var department in departments)
            {
                if(department.Id == departmentId)
                {
                    return department;
                }
            }

            return null;
        }
    }
}
