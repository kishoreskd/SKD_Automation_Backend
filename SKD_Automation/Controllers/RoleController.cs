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
using AM.Domain.Dto;
using AM.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using SKD_Automation.Filters;

namespace SKD_Automation.Controllers
{
    [ServiceFilter(typeof(HeaderAuthorizationFilterForLogin))]
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<Role> _validator;

        public RoleController(IUnitWorkService service, IMapper mapper, IValidator<Role> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllPlugin()
        {
            IEnumerable<Role> roles = await _service.Role.GetAll();

            if (!COM.IsAny(roles))
            {
                return NotFound();
            }

            return Ok(roles);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetSelected(int id)
        {
            Role role = await _service.Role.GetFirstOrDefault(e => e.RoleId.Equals(id));

            if (COM.IsNull(role))
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpPost("post")]
        public async Task<IActionResult> AddPlugin(Role role)
        {
            ValidationResult vResult = _validator.Validate(role);

            if (!vResult.IsValid)
            {
                return BadRequest(vResult);
            }

            await _service.Role.Add(role);
            await _service.Commit();
            return StatusCode(200);
        }

        [HttpPut("put/{id}")]
        public async Task<IActionResult> UpdatePlugin(int id, Role role)
        {
            Role exstingRole = await _service.Role.GetFirstOrDefault(e => e.RoleId.Equals(id), noTracking: false);

            if (COM.IsNull(exstingRole))
            {
                return NotFound();
            }

            ValidationResult vResult = _validator.Validate(role);

            if (!vResult.IsValid)
            {
                return BadRequest(vResult);
            }

            exstingRole.RoleName = role.RoleName;

            //_service.Plugin.Update(plgn);
            await _service.Commit();
            return StatusCode(200);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Role role = await _service.Role.GetFirstOrDefault(e => e.RoleId.Equals(id));
            User login = await _service.User.GetFirstOrDefault(e => e.RoleId.Equals(id));

            if (!COM.IsNull(login))
            {
                throw new DeleteFailureException("Role", id, "There are login are associated with Role!");
            }

            if (COM.IsNull(role))
            {
                return NotFound();
            }

            _service.Role.Remove(role);
            await _service.Commit();
            return StatusCode(200);
        }
    }
}
