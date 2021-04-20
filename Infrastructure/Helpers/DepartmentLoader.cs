using Core.Models;
using Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public class DepartmentLoader
    {
        public async Task<List<Department>> LoadAllDepartments()
        {
            var departmentRepository = new DepartmentRepository();
            return await departmentRepository.SelectAllAsync();
        }

        public Department LoadCurrentDepartment(List<Department> departments, string departmentName)
        {
            foreach (var department in departments)
            {
                if(department.Name.Equals(departmentName))
                {
                    return department;
                }
            }

            return null;
        }
    }
}
