using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWebApiEmpDepDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularWebApiEmpDepDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        [Route("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _departmentRepository.GetDepartments();
                if(departments == null)
                {
                    return NotFound();
                }

                return Ok(departments);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetDepartment")]
        public async Task<IActionResult> GetDepartment(int? departmentId)
        {
            if(departmentId == null)
            {
                return BadRequest();
            }
            try
            {
                var department = await _departmentRepository.GetDepartment(departmentId);
                if(department == null)
                {
                    return NotFound();
                }
                return Ok(department);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody]Department model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var departmentId = await _departmentRepository.AddDepartment(model);
                    if (departmentId > 0)
                        return Ok(departmentId);
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
        [Route("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment(int? departmentId)
        {
            int result = 0;
            if(departmentId == null)
            {
                return BadRequest();
            }
            try
            {
                result = await _departmentRepository.DeleteDepartment(departmentId);
                if(result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment(int? id, [FromBody]Department model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentRepository.UpdateDepartment(id, model);
                    return Ok("Updated Succesfully");
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
