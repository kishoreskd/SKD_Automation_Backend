﻿using Microsoft.AspNetCore.Http;
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

namespace SKD_Automation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authenticate : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<Department> _validator;
        private readonly string _plgnIncludeEntities;


        public Authenticate(IUnitWorkService service, IMapper mapper, IValidator<Department> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
            _plgnIncludeEntities = $"{nameof(Plugin.Department)},{nameof(Plugin.PluginLogCol)}";
        }


        [HttpGet("dashbord/{departmentid}")]
        public async Task<IActionResult> A()
        {
            IEnumerable<Plugin> pCol = await _service.Plugin.GetAll(e => e.DepartmentId.Equals(departmentid), includeProp: _plgnIncludeEntities);

            //List<Dashbord> dCol = pCol.Select((e, i) => new Dashbord
            //{
            //    totalPlugins = pCol.Count()
            //    totalAutomatedMinutes = e.AutomatedMinutes * e.PluginLogCol.Count,
            //    totalManualMiniutes = e.ManualMinutes * e.PluginLogCol.Count

            //}).ToList();

            double mMinutes = 0;
            double aMinutes = 0;

            foreach (Plugin plgn in pCol)
            {
                aMinutes += plgn.AutomatedMinutes * plgn.PluginLogCol.Count;
                mMinutes += plgn.ManualMinutes * plgn.PluginLogCol.Count;
            }

            Dashbord d = new Dashbord
            {
                totalPlugins = pCol.Count(),
                totalManualMiniutes = mMinutes,
                totalAutomatedMinutes = aMinutes
            };

            return Ok(d);
        }      
    }
}
