using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AM.Domain.Entities;
using AM.Persistence;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Am.Persistence.Seeding;

namespace SKD_Automation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<Department> _validator;

        public DepartmentController(IUnitWorkService service, IMapper mapper, IValidator<Department> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet("get_all")]
        public async Task<IActionResult> GetAll()
        {
            SeedingSampleData d = new SeedingSampleData(_service);
            await d.SeedAllAsync();
            IEnumerable<Department> departmentCol = await _service.Department.GetAll();

            if (!COM.IsAny(departmentCol))
            {
                return NotFound();
            }

            return Ok(departmentCol);

        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetSelected(int id)
        {
            Department dep = await _service.Department.GetFirstOrDefault(e => e.DepartmentId.Equals(id));

            if (COM.IsNull(dep))
            {
                return NotFound();
            }

            return Ok(dep);
        }

        [HttpPost("add_department/{departmentName}")]
        public async Task<IActionResult> AddDepartment(string departmentName)
        {
            Department dep = new Department();
            dep.DepartmentName = departmentName;

            //ValidationResult vResult = _validator.Validate(dep);

            //if (!vResult.IsValid)
            //{
            //    return BadRequest(vResult);
            //}


            await _service.Department.Add(dep);
            await _service.Commit();
            return StatusCode(200);
        }


        [HttpPost("add_department")]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            //ValidationResult vResult = _validator.Validate(department);

            //if (!vResult.IsValid)
            //{
            //    return BadRequest(vResult);
            //}


            await _service.Department.Add(department);
            await _service.Commit();
            return StatusCode(200);
        }

        [HttpPut("update_department/{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            Department dep = await _service.Department.GetFirstOrDefault(e => e.DepartmentId.Equals(id), noTracking: false);

            if (COM.IsNull(dep))
            {
                return NotFound();
            }

            dep.DepartmentName = department.DepartmentName;
            await _service.Commit();
            return StatusCode(200);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            Department dep = await _service.Department.GetFirstOrDefault(e => e.DepartmentId.Equals(id));

            if (COM.IsNull(dep))
            {
                return NotFound();
            }

            _service.Department.Remove(dep);
            await _service.Commit();
            return StatusCode(200);
        }
    }
}
