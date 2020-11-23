using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngularWebApiEmpDepDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;

namespace AngularWebApiEmpDepDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private IHostingEnvironment hostingEnvironment;
        public EmployeeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("GetEmployees")]
        public async Task<List<Employee>> GetEmployees()
        {
            return await _appDbContext.Employee.ToListAsync();
        }

        [HttpGet]
        [Route("GetEmployee")]
        public async Task<ActionResult<Employee>> GetEmployee(int? employeeId)
        {
            if (employeeId == null)
            {
                return BadRequest();
            }
            try
            {
                var employee = await _appDbContext.Employee.FindAsync(employeeId);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _appDbContext.Employee.Add(employee);
                    var employeeId = await _appDbContext.SaveChangesAsync();

                    if (employeeId > 0)
                        //return "Added Succesfully";
                        return Ok(employeeId);

                    return NotFound();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int? employeeId)
        {
            if (employeeId == null)
            {
                return BadRequest();
            }
            try
            {
                var employee = await _appDbContext.Employee.FindAsync(employeeId);
                if(employee == null)
                {
                    return NotFound();
                }

                _appDbContext.Employee.Remove(employee);
                await _appDbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, Employee employee)
        {
            if(employeeId != employee.EmployeeId)
            {
                return BadRequest();
            }

            //here we can update data in two ways. One is as below just one line.

            //_appDbContext.Entry(employee).State = EntityState.Modified;

            //Second way is this as below, update each filed indiviudally. Both are correct you can either use.

            var employeeval = await _appDbContext.Employee.FindAsync(employeeId);
            if(employeeval == null)
            {
                return NotFound();
            }

            employeeval.EmployeeName = employee.EmployeeName;
            employeeval.DateOfJoining = employee.DateOfJoining;
            employeeval.Department = employee.Department;
            employeeval.PhotoFileName = employee.PhotoFileName;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employeeId))
                {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _appDbContext.Employee.Any(e => e.EmployeeId == id);
        }

        [HttpGet]
        [Route("GetAllDepartmentNames")]
        public async Task<List<Department>> GetAllDepartmentNames()
        {
            return await _appDbContext.Departments.ToListAsync();
        }

        [HttpPost]
        [Route("UploadFile")]
        public IActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                var foldername = Path.Combine("Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                if(file.Length > 0)
                {
                    var fileName = file.FileName;
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(foldername, fileName);

                    using( var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                } 
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
                throw;
            }
        }
    }
}
