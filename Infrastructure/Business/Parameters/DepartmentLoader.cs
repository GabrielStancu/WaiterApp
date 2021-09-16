using Core.Models;
using Infrastructure.Repositories;
using System.Collections.Generic;

namespace Infrastructure.Business.Parameters
{
    public class DepartmentLoader : IDepartmentLoader
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentLoader(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public List<Department> LoadAllDepartments()
        {
            return _departmentRepository.SelectAll();
        }

        public Department LoadCurrentDepartment(List<Department> departments, int departmentId)
        {
            foreach (var department in departments)
            {
                if (department.Id == departmentId)
                {
                    return department;
                }
            }

            return null;
        }
    }
}
