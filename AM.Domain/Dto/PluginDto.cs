using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Dto
{
    public class PluginDto
    {
        public int PluginId { get; set; }
        public string Name { get; set; }
        public double ManualHour { get; set; }
        public double AutomatedHour { get; set; }
        public string Description { get; set; }
        public int CreatedEmployeeId { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<PluginLogDto> PluginLogs { get; set; }
    }
}
