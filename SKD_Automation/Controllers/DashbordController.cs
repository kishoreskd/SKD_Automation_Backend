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

namespace SKD_Automation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashbordController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<Department> _validator;
        private readonly string _plgnIncludeEntities;


        public DashbordController(IUnitWorkService service, IMapper mapper, IValidator<Department> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
            _plgnIncludeEntities = $"{nameof(Plugin.Department)},{nameof(Plugin.PluginLogCol)}";
        }


        [HttpGet("get_all/departmentid={departmentid}")]
        public async Task<IActionResult> GetAll(int departmentid)
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


        [HttpGet("get_all_by_monthyear/departmentid={departmentid}&month={month}&year={year}")]
        public async Task<IActionResult> Get(int departmentId, int month, int year)
        {
            IEnumerable<Plugin> pCol = await _service.Plugin.Get(e => e.DepartmentId.Equals(departmentId) && e.CreatedDate.Month.Equals(month) && e.CreatedDate.Year.Equals(year), includeProp: _plgnIncludeEntities);

            double mMinutes = 0;
            double aMinutes = 0;

            foreach (Plugin plgn in pCol)
            {
                List<PluginLog> log = plgn.PluginLogCol.Where(e => e.CreatedDate.Month.Equals(month) && e.CreatedDate.Year.Equals(year)).ToList();

                aMinutes += plgn.AutomatedMinutes * log.Count;
                mMinutes += plgn.ManualMinutes * log.Count;
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
