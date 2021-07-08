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
