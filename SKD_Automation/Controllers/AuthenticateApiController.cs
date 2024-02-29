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
using System.Net;
using SKD_Automation.Helper;
using AM.Domain.Dto;

namespace SKD_Automation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateApiController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly string _plgnIncludeEntities;

        public AuthenticateApiController(IUnitWorkService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
            _plgnIncludeEntities = $"{nameof(Plugin.Department)},{nameof(Plugin.PluginLogCol)}";
        }


        [HttpPost("authenticate/{id}")]
        public async Task<IActionResult> Authenticate(int id)
        {
            Plugin plugin = await _service.Plugin.GetFirstOrDefault(e => e.PluginId.Equals(id), noTracking: false);

            if (COM.IsNull(plugin)) return BadRequest(new ApiError { ErrorCode = 400, ErrorMessage = "Plugin does not exist!" });

            plugin.PluginToken = TokenHelper.CreateJwtForLicense(plugin.PluginName);

            string accessToken = plugin.PluginToken;

            await _service.Commit();

            return Ok(new TokenApiDto
            {
                AccessToken = accessToken
            });
        }
    }
}
