using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Dto
{
    public class PluginDto : AuditableEntity
    {
        public int PluginId { get; set; }
        public string PluginName { get; set; }
        public double ManualMinutes { get; set; }
        public double AutomatedMinutes { get; set; }
        public string Description { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }       

        public List<PluginLogDto> PluginLogs { get; set; }
    }
}
