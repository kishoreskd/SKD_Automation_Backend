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
    public class DashbordController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<PluginLog> _validator;

        public DashbordController(IUnitWorkService service, IMapper mapper, IValidator<PluginLog> validator)
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

        [HttpGet("get/{pluginId}")]
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

        [HttpGet("get_all/pluginId={pluginId}&month={month}&year={year}")]
        public async Task<IActionResult> GetForMonthAndYear(int pluginId, int month, int year)
        {
            IEnumerable<PluginLog> pluginLog = await _service.PluginLog.GetAll(e =>
            e.PluginId.Equals(pluginId) &&
            e.CreatedDate.Month.Equals(month) &&
            e.CreatedDate.Year.Equals(year));

            IEnumerable<PluginLogDto> dto = pluginLog.Select(e => _mapper.Map<PluginLogDto>(e));

            if (!COM.IsAny(dto))
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [HttpGet("get_all/pluginId={pluginId}&year={year}")]
        public async Task<IActionResult> GetForyear(int pluginId, int year)
        {
            IEnumerable<PluginLog> pluginLog = await _service.PluginLog.GetAll(e =>
            e.PluginId.Equals(pluginId) &&
            e.CreatedDate.Year.Equals(year));

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

  
    }
}
