using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Dto
{
    public class PluginLogDto
    {
        public int PluginLogId { get; set; }
        public string JobName { get; set; }
        public string Description { get; set; }
        public int CreatedEmployeeId { get; set; }
    }
}
