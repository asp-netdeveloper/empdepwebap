using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularWebApiEmpDepDemo.Models
{
    public interface IDepartmentRepository
    {
        Task<int> AddDepartment(Department department);
        Task<List<Department>> GetDepartments();
        //public IEnumerable<Department> GetAll();
        //public List<Product> GetProducts();
        Task<Department> GetDepartment(int? departmentId);
        Task<int> UpdateDepartment(int? id, Department department);
        Task<int> DeleteDepartment(int? departmentId);



    }
}
