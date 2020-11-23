using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularWebApiEmpDepDemo.Models
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _appDbContext;

        public DepartmentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<int> AddDepartment(Department department) 
        // here we are using int because we are returning department id which has datatype of int so.
        {
            if(_appDbContext != null)
            {
                await _appDbContext.Departments.AddAsync(department);
                await _appDbContext.SaveChangesAsync();

                return department.DepartmentId;
            }

            return 0;
        }

        public async Task<int> DeleteDepartment(int? departmentId)
        {
            int result = 0;

            if(_appDbContext != null)
            {
                var department = await _appDbContext.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

                if(department != null)
                {
                    _appDbContext.Departments.Remove(department);
                    result = await _appDbContext.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }

        public async Task<Department> GetDepartment(int? departmentId)
        {
            if(_appDbContext != null)
            {
                return await _appDbContext.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
            }

            return null;
        }

        public async Task<List<Department>> GetDepartments()
        {
            if(_appDbContext != null)
            {
                //return await _appDbContext.Departments.ToList();
                return await _appDbContext.Departments.ToListAsync();

                //here we are using async so we use tolistasync, if we use simple method type then just use tolist
            }

            return null;
        }

        public async Task<int> UpdateDepartment(int? id, Department department)
        {
            if (_appDbContext != null)
            {
                var departmentval = await _appDbContext.Departments.FindAsync(id);

                if (departmentval != null)
                {
                    departmentval.DepartmentName = department.DepartmentName;
                }

                try
                {
                    await _appDbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }

                return department.DepartmentId;
            }
            return 0;
        }
    }
}
