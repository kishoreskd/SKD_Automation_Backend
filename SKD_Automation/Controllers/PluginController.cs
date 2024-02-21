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

namespace SKD_Automation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PluginController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<Plugin> _validator;
        private readonly string _plgnIncludeEntities;


        public PluginController(IUnitWorkService service, IMapper mapper, IValidator<Plugin> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
            _plgnIncludeEntities = $"{nameof(Plugin.Department)},{nameof(Plugin.PluginLogCol)}";
        }


        [HttpGet("get_all")]
        public async Task<IActionResult> GetAllPlugin()
        {
            IEnumerable<Plugin> plugin = await _service.Plugin.GetAll(includeProp: $"{nameof(Plugin.Department)}");
            IEnumerable<PluginDto> dto = plugin.Select(e => _mapper.Map<PluginDto>(e));

            if (!COM.IsAny(dto))
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetSelected(int id)
        {
            Plugin plgn = await _service.Plugin.GetFirstOrDefault(e => e.PluginId.Equals(id), includeProp: _plgnIncludeEntities);
            PluginDto dto = _mapper.Map<PluginDto>(plgn);

            if (COM.IsNull(dto))
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [HttpPost("add_plugin")]
        public async Task<IActionResult> AddPlugin(PluginDto dto)
        {
            Plugin plgn = _mapper.Map<Plugin>(dto);
            ValidationResult vResult = _validator.Validate(plgn);

            if (!vResult.IsValid)
            {
                return BadRequest(vResult);
            }

            plgn.CreatedBy = dto.CreatedBy;
            plgn.CreatedDate = dto.CreatedDate;

            await _service.Plugin.Add(plgn);
            await _service.Commit();
            return StatusCode(200);
        }

        [HttpPut("update_plugin/{id}")]
        public async Task<IActionResult> UpdatePlugin(int id, PluginDto dto)
        {
            Plugin existingPlgn = await _service.Plugin.GetFirstOrDefault(e => e.PluginId.Equals(id), includeProp: _plgnIncludeEntities, noTracking: false);
            Plugin plgn = _mapper.Map(dto, existingPlgn);

            if (COM.IsNull(plgn))
            {
                return NotFound();
            }

            ValidationResult vResult = _validator.Validate(plgn);

            if (!vResult.IsValid)
            {
                return BadRequest(vResult);
            }

            plgn.LastModifiedBy = dto.LastModifiedBy;
            plgn.LastModifiedDate = dto.LastModifiedDate;

            //_service.Plugin.Update(plgn);
            await _service.Commit();
            return StatusCode(200);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> DeletePlugin(int id)
        {
            Plugin plgn = await _service.Plugin.GetFirstOrDefault(e => e.PluginId.Equals(id));
            PluginLog log = await _service.PluginLog.GetFirstOrDefault(e => e.PluginId.Equals(id));

            if (!COM.IsNull(log))
            {
                throw new DeleteFailureException("Plugin log", id, "There are plugin logs are associated with plugin!");
            }

            if (COM.IsNull(plgn))
            {
                return NotFound();
            }

            _service.Plugin.Remove(plgn);
            await _service.Commit();
            return StatusCode(200);
        }






        [HttpGet("get_withlog/pluginId={pluginId}")]
        public async Task<IActionResult> GetPluginWithLog(int pluginId)
        {
            Plugin plugin = await _service.Plugin.GetFirstOrDefault(e => e.PluginId.Equals(pluginId), includeProp: _plgnIncludeEntities);
            PluginDto dto = _mapper.Map<PluginDto>(plugin);

            if (COM.IsNull(dto))
            {
                return NotFound();
            }

            return Ok(dto);
        }


        [HttpGet("get_withlog_by_monthandyear/departmentid={departmentid}&month={month}&year={year}")]
        public async Task<IActionResult> GetAllPlugin(int departmentid, int month, int year)
        {
            IEnumerable<Plugin> plugin = await _service.Plugin.GetAll(e => e.DepartmentId.Equals(departmentid), includeProp: _plgnIncludeEntities);

            plugin.ToList().ForEach(e =>
            {
                e.PluginLogCol = e.PluginLogCol?.Where(e => e.CreatedDate.Month.Equals(month) && e.CreatedDate.Year.Equals(year)).ToList();
            });

            //plugin.Select(e=> )

            IEnumerable<PluginDto> dto = plugin.Select(e =>
            {
                e.PluginLogCol = e.PluginLogCol.Where(x => x.CreatedDate.Year.Equals(year)).ToList();
                return _mapper.Map<PluginDto>(e);
            });

            if (!COM.IsAny(dto))
            {
                return NotFound();
            }

            return Ok(dto);
        }


        [HttpGet("get_withlog_by_year/departmentid={departmentid}&year={year}")]
        public async Task<IActionResult> GetAllPlugin(int departmentid, int year)
        {
            IEnumerable<Plugin> plugin = await _service.Plugin.GetAll(e => e.DepartmentId.Equals(departmentid), includeProp: _plgnIncludeEntities);

            plugin.ToList().ForEach(e =>
            {

                e.PluginLogCol = e.PluginLogCol?.Where(e => e.CreatedDate.Year.Equals(year)).ToList();
            });


            IEnumerable<PluginDto> dto = plugin.Select(e => _mapper.Map<PluginDto>(e));

            if (!COM.IsAny(dto))
            {
                return NotFound();
            }

            return Ok(dto);
        }
    }
}
