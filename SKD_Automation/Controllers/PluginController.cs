using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKD_Automation.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PluginController : ControllerBase
    {
        [HttpGet("{id}")]
        public IEnumerable<string> Plugin()
        {
            return new List<string> { "Hello", "Hi" }.ToArray();
        }
    }
}
