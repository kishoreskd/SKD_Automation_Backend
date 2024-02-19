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
    public class PluginLogController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<PluginLog> _validator;

        public PluginLogController(IUnitWorkService service, IMapper mapper, IValidator<PluginLog> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet("get_all")]
        public async Task<IActionResult> GetAllPluginLog()
        {
            IEnumerable<PluginLog> pluginLog = await _service.PluginLog.GetAll();
            IEnumerable<PluginLogDto> dto = pluginLog.Select(e => _mapper.Map<PluginLogDto>(e));

            if (!COM.IsAny(dto))
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [HttpGet("get_all/{pluginId}")]
        public async Task<IActionResult> GetAllPluginLog(int pluginId)
        {
            IEnumerable<PluginLog> pluginLog = await _service.PluginLog.GetAll(e => e.PluginId.Equals(pluginId));
            IEnumerable<PluginLogDto> dto = pluginLog.Select(e => _mapper.Map<PluginLogDto>(e));

            if (!COM.IsAny(dto))
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetSelected(int id)
        {
            PluginLog plgnLog = await _service.PluginLog.GetFirstOrDefault(e => e.PluginLogId.Equals(id));
            PluginLogDto dto = _mapper.Map<PluginLogDto>(plgnLog);

            if (COM.IsNull(dto))
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [HttpPost("add_pluginlog")]
        public async Task<IActionResult> AddPluginLog(PluginLogDto dto)
        {
            PluginLog plgnLog = _mapper.Map<PluginLog>(dto);
            ValidationResult vResult = _validator.Validate(plgnLog);

            if (!vResult.IsValid)
            {
                return BadRequest(vResult);
            }

            plgnLog.CreatedBy = dto.CreatedBy;
            plgnLog.CreatedDate = dto.CreatedDate;

            await _service.PluginLog.Add(plgnLog);
            await _service.Commit();
            return StatusCode(200);
        }

        [HttpPut("update_pluginlog/{id}")]
        public async Task<IActionResult> UpdatePluginLog(int id, PluginLogDto dto)
        {
            PluginLog existingPlgnlog = await _service.PluginLog.GetFirstOrDefault(e => e.PluginLogId.Equals(id), noTracking: false);
            PluginLog plgnLog = _mapper.Map(dto, existingPlgnlog);

            if (COM.IsNull(plgnLog))
            {
                return NotFound();
            }

            ValidationResult vResult = _validator.Validate(plgnLog);

            if (!vResult.IsValid)
            {
                return BadRequest(vResult);
            }

            plgnLog.LastModifiedBy = dto.LastModifiedBy;
            plgnLog.LastModifiedDate = dto.LastModifiedDate;

            await _service.Commit();
            return StatusCode(200);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> DeletePluginLog(int id)
        {
            PluginLog plgnLog = await _service.PluginLog.GetFirstOrDefault(e => e.PluginLogId.Equals(id));

            if (COM.IsNull(plgnLog))
            {
                return NotFound();
            }

            Plugin plgn = await _service.Plugin.GetFirstOrDefault(e => e.PluginId.Equals(plgnLog.PluginId));

            if (!COM.IsNull(plgn))
            {
                throw new DeleteFailureException("Plugin log", id, "There is one plugin is associated with this plugin log!");
            }

            _service.PluginLog.Remove(plgnLog);
            await _service.Commit();
            return StatusCode(200);
        }
    }
}
