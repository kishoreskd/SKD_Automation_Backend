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
using SKD_Automation.Filters;

namespace SKD_Automation.Controllers
{
    [ServiceFilter(typeof(HeaderAuthorizationFilterForLogin))]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateApiController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _tokenHelper;
        private readonly string _plgnIncludeEntities;

        public AuthenticateApiController(IUnitWorkService service, IMapper mapper, ITokenHelper tokenHelper)
        {
            _service = service;
            _mapper = mapper;
            _plgnIncludeEntities = $"{nameof(Plugin.Department)},{nameof(Plugin.PluginLogCol)}";
            _tokenHelper = tokenHelper;
        }


        [HttpGet("authenticate/{id:int}")]
        public async Task<IActionResult> Authenticate(int id)
        {
            try
            {
                Plugin plugin = await _service.Plugin.GetFirstOrDefault(e => e.PluginId.Equals(id), noTracking: false);

                if (COM.IsNull(plugin)) return BadRequest(new ApiError { ErrorCode = 400, ErrorMessage = "Plugin does not exist!" });

                string key = EncryptionLibraryHelper.EncryptText(plugin.PluginId.ToString());

                if (COM.IsNullOrEmpty(key))
                {
                    return BadRequest(new ApiError { ErrorCode = 400, ErrorMessage = "Key generation failed!" });
                }

                string accessToken = _tokenHelper.CreateJwtForLicense(plugin.PluginName, key);

                return Ok(new TokenApiDto
                {
                    AccessToken = accessToken
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
