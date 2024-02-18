﻿using Microsoft.AspNetCore.Mvc;
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

            await _service.PluginLog.Add(plgnLog);
            await _service.Commit();
            return StatusCode(200);
        }

        [HttpPut("update_pluginlog/{id}")]
        public async Task<IActionResult> UpdatePluginLog(int id, PluginLogDto dto)
        {
            PluginLog existingPlgn = await _service.PluginLog.GetFirstOrDefault(e => e.PluginLogId.Equals(id));
            PluginLog plgnLog = _mapper.Map<PluginLog>(dto);

            if (COM.IsNull(plgnLog))
            {
                return NotFound();
            }

            ValidationResult vResult = _validator.Validate(plgnLog);

            if (!vResult.IsValid)
            {
                return BadRequest(vResult);
            }

            plgnLog.LastModifiedBy = id;
            _service.PluginLog.Update(plgnLog);
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

            _service.PluginLog.Remove(plgnLog);
            await _service.Commit();
            return StatusCode(200);
        }
    }
}
