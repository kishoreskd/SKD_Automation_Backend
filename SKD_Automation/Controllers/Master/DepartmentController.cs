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
using Microsoft.AspNetCore.Authorization;
using SKD_Automation.Filters;
using Microsoft.Extensions.Options;
using SKD_Automation.Helper;
using AM.Application.Exceptions;

namespace SKD_Automation.Controllers
{
    //[ServiceFilter(typeof(HeaderAuthorizationFilterForLogin))]
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<Department> _validator;

        public DepartmentController(IUnitWorkService service, IMapper mapper, IValidator<Department> validator, IOptions<JwtAppSettingJson> jwtSetting)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet("get_all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //SeedingSampleData d = new SeedingSampleData(_service);
                //await d.SeedAllAsync();


                IEnumerable<Department> departmentCol = await _service.Department.GetAll();

                if (!COM.IsAny(departmentCol))
                {
                    return NotFound();
                }

                return Ok(departmentCol);
            }
            catch (Exception ex)
            {

                throw;
            }

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

        [HttpPost("post")]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            if (COM.IsNull(department))
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = 400,
                    ErrorMessage = "Department should not be null!"
                });
            }

            if (COM.IsNullOrEmpty(department.DepartmentName))
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = 400,
                    ErrorMessage = "Department name can't be empty!"
                });
            }

            if (await CheckDepNameExistAsync(department.DepartmentName))
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = 400,
                    ErrorMessage = "Department Name already exist!"
                });
            }


            await _service.Department.Add(department);
            await _service.Commit();
            return StatusCode(200);
        }

        [HttpPut("put/{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            Department dep = await _service.Department.GetFirstOrDefault(e => e.DepartmentId.Equals(id));
            string existingDepName = dep.DepartmentName;

            if (COM.IsNull(dep))
            {
                return NotFound();
            }

            if (!existingDepName.Equals(department.DepartmentName))
            {
                if (await CheckDepNameExistAsync(department.DepartmentName))
                {
                    return BadRequest(new ApiError
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Department Name already exist!"
                    });
                }
            }

            _service.Department.Update(department);
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

            bool hasAssociated = await _service.Plugin.IsAnyAsync(e => e.DepartmentId.Equals(id));

            if (hasAssociated)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = 400,
                    ErrorMessage = "This department is associated with plugins, you can't able to remove!"
                });
            }

            _service.Department.Remove(dep);
            await _service.Commit();
            return StatusCode(200);
        }

        private async Task<bool> CheckDepNameExistAsync(string departmentName) => await _service.Department.IsAnyAsync(e => e.DepartmentName.Equals(departmentName));
    }
}
