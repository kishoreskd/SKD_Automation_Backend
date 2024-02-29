using AM.Domain.Dto;
using AM.Domain.Entities;
using AM.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SKD_Automation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogApiController : ControllerBase
    {
        private readonly IUnitWorkService _service;

        public LogApiController(IUnitWorkService service)
        {
            _service = service;
        }

        [HttpPost("post/log")]
        public async Task<IActionResult> AddLog(LogApiDto dto)
        {
            var clientHeader = HttpContext.Request.Headers["Auth-Key"];
            string token = HttpContext.Request.Headers["Auth-Key"].ToString().Replace("Bearer ", "");

            Plugin p = await _service.Plugin.GetFirstOrDefault(e => e.PluginToken.Equals(token));

            if (p is null) return NotFound(new ApiError
            {
                ErrorCode = 404,
                ErrorMessage = "Not found!"
            });

            await _service.PluginLog.Add(new PluginLog
            {
                JobName = dto.JobName,
                Activity = dto.Activity,
                PluginId = p.PluginId,
                CreatedBy = dto.EmployeeId,
            });

            await _service.Commit();

            return Ok();
        }
    }
}
