using AM.Domain.Dto;
using AM.Domain.Entities;
using AM.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SKD_Automation.Filters;
using SKD_Automation.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SKD_Automation.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(HeaderAuthorizationFilterForLicense))]
    [ApiController]
    public class LogApiController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly ITokenHelper _tokenHelper;

        public LogApiController(IUnitWorkService service, ITokenHelper tokenHelper)
        {
            _service = service;
            this._tokenHelper = tokenHelper;
        }

        [HttpPost("post/log")]
        public async Task<IActionResult> AddLog(LogApiDto dto)
        {
            var clientHeader = HttpContext.Request.Headers[SD.HeadersKey.License];
            string token = HttpContext.Request.Headers[SD.HeadersKey.License].ToString().Replace("Bearer ", "");

            ClaimsPrincipal principal = _tokenHelper.GetPrincipleForLicense(token);

            string key = principal.Claims.FirstOrDefault(e => e.Type == "key").Value.ToString();

            string idKey = EncryptionLibraryHelper.DecryptText(key);

            int pluginId = Convert.ToInt32(idKey);

            Plugin p = await _service.Plugin.GetFirstOrDefault(e => e.PluginId.Equals(pluginId));

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
